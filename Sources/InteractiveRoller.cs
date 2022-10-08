using System.Collections.Immutable;

namespace RP.ReverieWorld.DiceRoll
{
    public class InteractiveRoller
    {
        enum State
        {
            Init,
            RemovingDices,
            Ready,
        }

        private State state = State.Init;

        private readonly IRandomProvider randomProvider;
        private readonly IParameters parameters;
        private readonly List<Dice> data;
        private Result? result;

        public IReadOnlyList<Dice> Values => data.ToImmutableList();

        public int DicesToRemove => parameters.AdditionalDicesCount - data.Where(d => d.Removed).Count();

        /// <summary>
        /// </summary>
        /// <param name="randomProvider">Implementation of <see cref="IRandomProvider"/> interface.</param>
        /// <param name="parameters">Custom implementation of <see cref="IParameters"/> interface or <see cref="Parameters"/> (default).</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="randomProvider"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public InteractiveRoller(IRandomProvider randomProvider, IParameters? parameters = null)
        {
            ArgumentNullException.ThrowIfNull(randomProvider);

            parameters ??= new Parameters();
            Parameters.Validate(parameters);

            this.randomProvider = randomProvider;
            this.parameters = parameters;
            this.data = new List<Dice>(parameters.DicesCount + parameters.AdditionalDicesCount + (parameters.HasInfinityBursts ? parameters.DicesCount : parameters.BurstsCount));
        }

        public InteractiveRollerDiceRemover Begin()
        {
            if (state != State.Init)
            {
                throw new InvalidOperationException();
            }

            int initialRollsCount = parameters.DicesCount + parameters.AdditionalDicesCount;

            using (var random = new RollMaker(this))
            {
                for (int i = 0; i != initialRollsCount; ++i)
                {
                    data.Add(new Dice(random.Next()));
                }
            }

            state = State.RemovingDices;

            return new InteractiveRollerDiceRemover(this);
        }

        public void RemoveDice(int index)
        {
            if (state != State.RemovingDices)
            {
                throw new InvalidOperationException();
            }

            if (index < 0 || data.Count <= index)
            {
                throw new ArgumentOutOfRangeException(nameof(index), index, "Index out of range");
            }

            data[index].Removed = true;
        }

        public void RemoveDices(IReadOnlySet<int> indices)
        {
            if (state != State.RemovingDices)
            {
                throw new InvalidOperationException();
            }

            ArgumentNullException.ThrowIfNull(indices);

            foreach (int index in indices)
            {
                if (index < 0 || data.Count <= index)
                {
                    throw new ArgumentOutOfRangeException(nameof(indices), indices, "One of indices out of range");
                }
            }

            foreach (int index in indices)
            {
                data[index].Removed = true;
            }
        }

        public Result Result()
        {
            if (state == State.RemovingDices)
            {
                if (DicesToRemove != 0)
                {
                    throw new InvalidOperationException();
                }

                CompleteRerrolsAndBursts();

                result = new Result(data, parameters);
                state = State.Ready;
            }

            if (state != State.Ready)
            {
                throw new InvalidOperationException();
            }

            return result!;
        }

        public class InteractiveRollerDiceRemover
        {
            private readonly InteractiveRoller source;

            public IReadOnlyList<Dice> Values => source.Values;

            /// <summary>
            /// </summary>
            /// <returns>Current state of the <see cref="Roll"/>.</returns>
            public Roll Current => source.result ?? new Roll(source.Values, source.parameters);

            public bool StageConditionsMet => source.DicesToRemove == 0;

            public void RemoveDice(int index)
            {
                source.RemoveDice(index);
            }

            public void RemoveDices(IReadOnlySet<int> indices)
            {
                source.RemoveDices(indices);
            }

            public Result Result()
            {
                return source.Result();
            }

            internal InteractiveRollerDiceRemover(InteractiveRoller source)
            {
                this.source = source;
            }
        }

        private void CompleteRerrolsAndBursts()
        {
            using var random = new RollMaker(this);

            bool somethingChanged = false;
            int availableRerolls = parameters.RerollsCount;
            int availableBursts = parameters.BurstsCount;
            do
            {
                somethingChanged = false;

                if (availableRerolls != 0)
                {
                    var rerollsCount = parameters.HasInfinityRerolls ? int.MaxValue : availableRerolls;
                    foreach (var d in data.Where(d => !d.Removed && d.Value == 1).Take(rerollsCount))
                    {
                        if (!parameters.HasInfinityRerolls)
                        {
                            --availableRerolls;
                        }

                        d.values.Add(random.Next());
                        somethingChanged = true;
                    }
                }

                if (availableBursts != 0)
                {
                    var burstsCount = parameters.HasInfinityBursts ? int.MaxValue : availableBursts;
                    var toBurst = data.Where(d => !d.Removed && !d.burstMade && d.Value == parameters.FacesCount).Take(burstsCount);
                    List<Dice> newRolls = new(toBurst.Count());

                    foreach (var d in toBurst)
                    {
                        if (!parameters.HasInfinityBursts)
                        {
                            --availableBursts;
                        }

                        d.burstMade = true;

                        newRolls.Add(new Dice(random.Next(), isBurst: true));
                        somethingChanged = true;
                    }

                    data.AddRange(newRolls);
                }
            } while (somethingChanged);
        }

        private struct RollMaker : IDisposable
        {
            private readonly int facesCount;
            private readonly IRandom random;

            internal RollMaker(InteractiveRoller roller)
            {
                facesCount = roller.parameters.FacesCount;
                random = roller.randomProvider.Lock();
            }

            public int Next()
            {
                return random.Next(facesCount) + 1;
            }

            public void Dispose()
            {
                random.Dispose();
            }
        }
    }
}
