using System.Collections;
using System.Diagnostics;

namespace RP.ReverieWorld.DiceRoll
{
    public sealed partial class AutoRoller
    {
        private readonly IRandomProvider randomProvider;
        private readonly IParameters defaultParameters;
        private readonly IDiceRemovingSelector diceRemovingSelector;

        /// <summary>
        /// Strategy for "add then remove" dice mechanic.
        /// </summary>
        public interface IDiceRemovingSelector
        {
            /// <summary>
            /// </summary>
            /// <param name="dices">List of current dices values.</param>
            /// <param name="count">Count of dices to remove.</param>
            /// <param name="parameters">Parameters of current roll.</param>
            /// <returns>Set of indices that must be removed with exactly <paramref name="count"/> elements.</returns>
            IReadOnlySet<int> Select(IReadOnlyList<int> dices, int count, IParameters parameters);
        }

        /// <summary>
        /// </summary>
        /// <param name="randomProvider">Implementation of <see cref="IRandomProvider"/> interface.</param>
        /// <param name="defaultParameters">Custom implementation of <see cref="IParameters"/> interface or <see cref="Parameters"/> (default).</param>
        /// <param name="diceRemovingSelector">Custom implementation of <see cref="IDiceRemovingSelector"/> interface or <see cref="DefaultDiceRemovingSelector"/> (default).</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="randomProvider"/> is <see langword="null"/>.</exception>
        public AutoRoller(IRandomProvider randomProvider, IParameters? defaultParameters = null, IDiceRemovingSelector? diceRemovingSelector = null)
        {
            ArgumentNullException.ThrowIfNull(randomProvider);

            this.randomProvider = randomProvider;
            this.defaultParameters = defaultParameters ?? new Parameters();
            Parameters.Validate(this.defaultParameters);
            this.diceRemovingSelector = diceRemovingSelector ?? new DefaultDiceRemovingSelector();
        }

        /// <summary>
        /// </summary>
        /// <param name="randomProvider">Implementation of <see cref="IRandomProvider"/> interface.</param>
        /// <param name="diceRemovingSelector">Custom implementation of <see cref="IDiceRemovingSelector"/> interface or <see cref="DefaultDiceRemovingSelector"/> (default).</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="randomProvider"/> is <see langword="null"/>.</exception>
        public AutoRoller(IRandomProvider randomProvider, IDiceRemovingSelector? diceRemovingSelector) :
            this(randomProvider, null, diceRemovingSelector)
        {
        }

        [DebuggerDisplay("{Value}")]
        internal sealed class DiceData
        {
            public List<int> values;
            public bool removed;
            public bool burstMade;
            public bool isBurst;

            public int Value => values.Last();

            public DiceData(int value, bool removed = false, bool burstMade = false, bool isBurst = false)
            {
                this.values = new List<int>(1) { value };
                this.removed = removed;
                this.burstMade = burstMade;
                this.isBurst = isBurst;
            }
        }

        public sealed class Dice : IReadOnlyList<int>
        {
            private readonly DiceData data;

            public int Value { get; }
            public int RollsCount => data.values.Count;
            public bool WasRemoved => data.removed;
            public bool IsBurst => data.isBurst;

            internal Dice(DiceData data)
            {
                this.data = data;

                Value = data.Value;
            }

            public int this[int index] => data.values[index];

            public int Count => data.values.Count;

            public IEnumerator<int> GetEnumerator() => data.values.GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)data.values).GetEnumerator();

            public override string ToString()
            {
                return $"{(WasRemoved ? "-" : string.Empty)}{(IsBurst ? "*" : string.Empty)}{Value}";
            }
        }

        public sealed class Result : IReadOnlyList<Dice>
        {
            private readonly IReadOnlyList<Dice> rolls;

            private readonly IParameters parameters;

            public int Total { get; }
            public int Bonus => parameters.Bonus;

            public int DiceFacesCount => parameters.FacesCount;
            public int BaseDicesCount => parameters.DicesCount;
            public int RemovedDicesCount => parameters.AdditionalDicesCount;
            public int InitialRerollsCount => parameters.RerollsCount;
            public int InitialBurstsCount => parameters.BurstsCount;

            public bool HasInfinityRerolls => parameters.HasInfinityRerolls;
            public bool HasInfinityBursts => parameters.HasInfinityBursts;

            internal Result(List<DiceData> data, IParameters parameters)
            {
                this.rolls = data.Select(d => new Dice(d)).ToArray();
                this.parameters = parameters;

                Total = rolls.Where(d => !d.WasRemoved).Sum(d => d.Value) + Bonus;
            }

            public Dice this[int index] => rolls[index];

            public int Count => rolls.Count;

            public IEnumerator<Dice> GetEnumerator() => rolls.GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)rolls).GetEnumerator();

            public override string ToString()
            {
                return Total.ToString();
            }
        }

        public Result Roll(IParameters? parameters = null)
        {
            parameters ??= defaultParameters;
            Parameters.Validate(parameters);

            List<DiceData> data = new(parameters.DicesCount + parameters.AdditionalDicesCount + (parameters.HasInfinityBursts ? parameters.DicesCount : parameters.BurstsCount));

            using (var random = randomProvider.Lock())
            {
                int makeRoll()
                {
                    return random.Next(parameters.FacesCount) + 1;
                }

                {
                    int initialRollsCount = parameters.DicesCount + parameters.AdditionalDicesCount;
                    for (int i = 0; i != initialRollsCount; ++i)
                    {
                        data.Add(new DiceData(makeRoll()));
                    }
                }

                {
                    var indicesToRemove = diceRemovingSelector.Select(data.Select(d => d.Value).ToArray(), parameters.AdditionalDicesCount, parameters);
                    if (indicesToRemove.Count != parameters.AdditionalDicesCount)
                    {
                        throw new InvalidOperationException("Count of elements from diceRemovingSelector.Select() must be equal to count of additional dices in roll");
                    }

                    foreach (var i in indicesToRemove)
                    {
                        data[i].removed = true;
                    }
                }

                {
                    bool somethingChanged = false;
                    int availableRerolls = parameters.RerollsCount;
                    int availableBursts = parameters.BurstsCount;
                    do
                    {
                        somethingChanged = false;

                        if (availableRerolls != 0)
                        {
                            var rerollsCount = parameters.HasInfinityRerolls ? int.MaxValue : availableRerolls;
                            foreach (var d in data.Where(d => !d.removed && d.Value == 1).Take(rerollsCount))
                            {
                                if (!parameters.HasInfinityRerolls)
                                {
                                    --availableRerolls;
                                }

                                d.values.Add(makeRoll());
                                somethingChanged = true;
                            }
                        }

                        if (availableBursts != 0)
                        {
                            var burstsCount = parameters.HasInfinityBursts ? int.MaxValue : availableBursts;
                            var toBurst = data.Where(d => !d.removed && !d.burstMade && d.Value == parameters.FacesCount).Take(burstsCount);
                            var newRolls = new List<DiceData>(toBurst.Count());

                            foreach (var d in toBurst)
                            {
                                if (!parameters.HasInfinityBursts)
                                {
                                    --availableBursts;
                                }

                                d.burstMade = true;

                                newRolls.Add(new DiceData(makeRoll(), isBurst: true));
                                somethingChanged = true;
                            }

                            data.AddRange(newRolls);
                        }
                    } while (somethingChanged);
                }
            }

            return new Result(data, parameters);
        }
    }
}
