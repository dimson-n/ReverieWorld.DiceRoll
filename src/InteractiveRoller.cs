namespace RP.ReverieWorld.DiceRoll;

/// <summary>
/// Represents an interactive dice roller.
/// </summary>
public sealed class InteractiveRoller
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

    /// <summary>
    /// Gets a readonly list of dices of current state.
    /// </summary>
    /// <returns>A <see cref="IReadOnlyList{T}"/> of <see cref="Dice"/>s.</returns>
    public IReadOnlyList<Dice> Values => data.AsReadOnly();

    /// <summary>
    /// Gets current state of the <see cref="Roll"/>.
    /// </summary>
    /// <returns>Current state of the <see cref="Roll"/>.</returns>
    public Roll Current => result ?? new Roll(data, parameters);

    /// <summary>
    /// Gets count of <see cref="Dice"/>s that need to be removed from roll.
    /// </summary>
    /// <returns>Count of <see cref="Dice"/>s that need to be removed from roll.</returns>
    public int DicesToRemove => parameters.AdditionalDicesCount - data.Where(d => d.Removed).Count();

    /// <summary>
    /// Initializes a new instance of the <see cref="InteractiveRoller"/> with specified <paramref name="randomProvider"/> and optional <paramref name="parameters"/>.
    /// </summary>
    /// <param name="randomProvider">Implementation of <see cref="IRandomProvider"/> interface.</param>
    /// <param name="parameters">Custom implementation of <see cref="IParameters"/> interface or <see cref="Parameters"/> (default).</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="randomProvider"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    /// <exception cref="ArgumentException"></exception>
    public InteractiveRoller(IRandomProvider randomProvider, IParameters? parameters = null)
    {
        ArgumentNullException.ThrowIfNull(randomProvider);

        parameters ??= Parameters.Default;
        Parameters.Validate(parameters);

        this.randomProvider = randomProvider;
        this.parameters = parameters;
        this.data = new List<Dice>(parameters.DicesCount + parameters.AdditionalDicesCount + (parameters.HasInfinityBursts ? parameters.DicesCount : parameters.BurstsCount));
    }

    /// <summary>
    /// Performs initial dice roll.
    /// </summary>
    /// <returns>Next stage wrapper.</returns>
    /// <exception cref="InvalidOperationException"></exception>
    public DiceRemoveStage Begin()
    {
        if (state != State.Init)
        {
            throw new InvalidOperationException();
        }

        int initialRollsCount = parameters.DicesCount + parameters.AdditionalDicesCount;

        using (RollMaker random = new(this))
        {
            for (int i = 0; i != initialRollsCount; ++i)
            {
                data.Add(new Dice(random.Next()));
            }
        }

        state = State.RemovingDices;

        return new DiceRemoveStage(this);
    }

    /// <summary>
    /// Removes a <see cref="Dice"/> at the given <paramref name="index"/> from roll.
    /// </summary>
    /// <param name="index">Index of a <see cref="Dice"/> to remove.</param>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public void RemoveDice(int index)
    {
        if (state != State.RemovingDices)
        {
            throw new InvalidOperationException();
        }

        if (DicesToRemove <= 0)
        {
            throw new InvalidOperationException("No more dices to remove");
        }

        if (index < 0 || data.Count <= index)
        {
            throw new ArgumentOutOfRangeException(nameof(index), index, "Index out of range");
        }

        data[index].Removed = true;
    }

    /// <summary>
    /// Removes a set of <see cref="Dice"/>s at the given <paramref name="indices"/> from roll.
    /// </summary>
    /// <param name="indices">Set of <see cref="Dice"/> indices to remove.</param>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public void RemoveDices(IReadOnlySet<int> indices)
    {
        if (state != State.RemovingDices)
        {
            throw new InvalidOperationException();
        }

        ArgumentNullException.ThrowIfNull(indices);

        if (DicesToRemove < indices.Count)
        {
            throw new InvalidOperationException("Too many indices to remove provided");
        }

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

    /// <summary>
    /// Completes the dice roll interaction.
    /// </summary>
    /// <returns><see cref="DiceRoll.Result"/> of the dice roll.</returns>
    /// <exception cref="InvalidOperationException"></exception>
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

    /// <summary>
    /// Represents a dice remove stage of interactive roll.
    /// </summary>
    public sealed class DiceRemoveStage
    {
        private readonly InteractiveRoller source;

        /// <summary>
        /// Gets a readonly list of dices of current state.
        /// </summary>
        /// <returns>A <see cref="IReadOnlyList{T}"/> of <see cref="Dice"/>s.</returns>
        public IReadOnlyList<Dice> Values => source.Values;

        /// <summary>
        /// Gets current state of the <see cref="Roll"/>.
        /// </summary>
        /// <returns>Current state of the <see cref="Roll"/>.</returns>
        public Roll Current => source.Current;

        /// <summary>
        /// Gets count of dices to remove at this stage.
        /// </summary>
        /// <returns>Count of dices that needs to be removed from the <see cref="Roll"/>.</returns>
        public int DicesToRemove => source.DicesToRemove;

        /// <summary>
        /// Indicates that proper count of <see cref="Dice"/>s already removed from the <see cref="Roll"/>.
        /// </summary>
        /// <returns><see langword="true"/> if there is no <see cref="Dice"/>s to remove; otherwise <see langword="false"/>.</returns>
        public bool StageConditionsMet => source.DicesToRemove == 0;

        /// <summary>
        /// Removes a <see cref="Dice"/> at the given <paramref name="index"/> from roll.
        /// </summary>
        /// <param name="index">Index of a <see cref="Dice"/> to remove.</param>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public void RemoveDice(int index)
        {
            source.RemoveDice(index);
        }

        /// <summary>
        /// Removes a set of <see cref="Dice"/>s at the given <paramref name="indices"/> from roll.
        /// </summary>
        /// <param name="indices">Set of <see cref="Dice"/> indices to remove.</param>
        /// <exception cref="InvalidOperationException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public void RemoveDices(IReadOnlySet<int> indices)
        {
            source.RemoveDices(indices);
        }

        /// <summary>
        /// Completes the dice roll interaction.
        /// </summary>
        /// <returns><see cref="DiceRoll.Result"/> of the dice roll.</returns>
        /// <exception cref="InvalidOperationException"></exception>
        public Result Result()
        {
            return source.Result();
        }

        internal DiceRemoveStage(InteractiveRoller source)
        {
            this.source = source;
        }
    }

    private void CompleteRerrolsAndBursts()
    {
        bool somethingChanged = false;

        ParamCounter availableRerolls = new(parameters.RerollsCount, parameters.HasInfinityRerolls);
        ParamCounter availableBursts  = new(parameters.BurstsCount,  parameters.HasInfinityBursts);

        int currentRound = 0;

        using RollMaker random = new(this);
        do
        {
            somethingChanged = false;

            if (availableRerolls.Exists)
            {
                var toReroll = data.Where(d => !d.Removed && d.Value == 1);
                while (toReroll.Any() && availableRerolls.Exists)
                {
                    foreach (var d in toReroll.Take(availableRerolls.MaxCount))
                    {
                        --availableRerolls;
                        d.values.Add(random.Next());
                    }

                    somethingChanged = true;
                    ++currentRound;
                }
            }

            if (availableBursts.Exists)
            {
                var toBurst = data.Where(d => !d.Removed && !d.burstMade && d.Value == parameters.FacesCount);
                List<Dice> newRolls = new(toBurst.Count());

                while (toBurst.Any() && availableBursts.Exists)
                {
                    newRolls.Clear();
                    foreach (var d in toBurst.Take(availableBursts.MaxCount))
                    {
                        newRolls.Add(new Dice(random.Next(), offset: currentRound, isBurst: true));
                        d.burstMade = true;
                        --availableBursts;
                    }
                    data.AddRange(newRolls);

                    somethingChanged = true;
                }
            }
        } while (somethingChanged);
    }

    private readonly struct RollMaker : IDisposable
    {
        private readonly int facesCount;
        private readonly IRandom random;

        internal RollMaker(InteractiveRoller roller)
        {
            facesCount = roller.parameters.FacesCount;
            random = roller.randomProvider.Lock();
        }

        public readonly int Next()
        {
            return random.Next(facesCount) + 1;
        }

        public void Dispose()
        {
            random.Dispose();
        }
    }

    private class ParamCounter
    {
        private int count;
        private readonly bool infinity;

        internal ParamCounter(int count, bool infinity)
        {
            this.count = count;
            this.infinity = infinity;
        }

        public bool Exists => infinity || count != 0;

        public int MaxCount => infinity ? int.MaxValue : count;

        public static ParamCounter operator --(ParamCounter counter)
        {
            if (!counter.infinity)
            {
                --counter.count;
            }
            return counter;
        }
    }
}
