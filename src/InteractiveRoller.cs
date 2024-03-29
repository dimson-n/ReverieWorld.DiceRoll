namespace ReverieWorld.DiceRoll;

/// <summary>
/// Represents an interactive dice roller.
/// </summary>
/// <seealso cref="AutoRoller"/>
public sealed class InteractiveRoller
{
    enum Stage
    {
        Init,
        RemovingDices,
        Ready,
    }

    private Stage stage = Stage.Init;

    private readonly RollState state;

    private Result? result;

    /// <summary>
    /// Gets a read-only list of dices of current state.
    /// </summary>
    /// <value>A <see cref="IReadOnlyList{T}"/> of <see cref="Dice"/>s.</value>
    public IReadOnlyList<Dice> Values => state.Values;

    /// <summary>
    /// Gets current state of the <see cref="Roll"/>.
    /// </summary>
    /// <value>Current state of the <see cref="Roll"/>.</value>
    public Roll Current => result ?? new Roll(state);

    /// <summary>
    /// Gets count of <see cref="Dice"/>s that need to be removed from roll.
    /// </summary>
    /// <value>Count of <see cref="Dice"/>s that need to be removed from roll.</value>
    public int DicesToRemove => state.DicesToRemove;

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
        parameters.Validate();

        this.state = new(parameters, randomProvider);
    }

    /// <summary>
    /// Performs initial dice roll.
    /// </summary>
    /// <returns>Next stage wrapper.</returns>
    /// <exception cref="InvalidOperationException"></exception>
    public DiceRemoveStage Begin()
    {
        if (stage != Stage.Init)
        {
            throw new InvalidOperationException("Begin can be called once after initialization only");
        }

        state.FillInitial();

        stage = Stage.RemovingDices;

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
        if (stage != Stage.RemovingDices)
        {
            throw new InvalidOperationException("Can't remove dices at current stage");
        }

        if (DicesToRemove <= 0)
        {
            throw new InvalidOperationException("No more dices to remove");
        }

        if (index < 0 || state.Count <= index)
        {
            throw new ArgumentOutOfRangeException(nameof(index), index, "Index out of range");
        }

        state[index].Removed = true;
    }

    /// <summary>
    /// Removes a set of <see cref="Dice"/>s at the given <paramref name="indices"/> from roll.
    /// </summary>
    /// <param name="indices">Set of <see cref="Dice"/> indices to remove.</param>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public void RemoveDices(IReadOnlySet<int> indices)
    {
        ArgumentNullException.ThrowIfNull(indices);

        if (stage != Stage.RemovingDices)
        {
            throw new InvalidOperationException("Can't remove dices at current stage");
        }

        state.RemoveDices(indices);
    }

    /// <summary>
    /// Completes the dice roll interaction.
    /// </summary>
    /// <returns><see cref="DiceRoll.Result"/> of the dice roll.</returns>
    /// <exception cref="InvalidOperationException"></exception>
    public Result Result()
    {
        if (stage == Stage.RemovingDices)
        {
            if (DicesToRemove != 0)
            {
                throw new InvalidOperationException("More dices need to be removed");
            }

            state.CompleteRerollsAndBursts();

            result = new Result(state);
            stage = Stage.Ready;
        }

        if (stage != Stage.Ready)
        {
            throw new InvalidOperationException("Can't get result at current stage");
        }

        return result!;
    }

    /// <summary>
    /// Represents a dice remove stage of interactive roll.
    /// </summary>
    public sealed class DiceRemoveStage
    {
        private readonly InteractiveRoller source;

        /// <inheritdoc cref="InteractiveRoller.Values"/>
        public IReadOnlyList<Dice> Values => source.Values;

        /// <inheritdoc cref="InteractiveRoller.Current"/>
        public Roll Current => source.Current;

        /// <inheritdoc cref="InteractiveRoller.DicesToRemove"/>
        public int DicesToRemove => source.DicesToRemove;

        /// <summary>
        /// Indicates that proper count of <see cref="Dice"/>s already removed from the <see cref="Roll"/>.
        /// </summary>
        /// <value><see langword="true"/> if there is no <see cref="Dice"/>s to remove; otherwise <see langword="false"/>.</value>
        public bool StageConditionsMet => source.DicesToRemove == 0;

        /// <inheritdoc cref="InteractiveRoller.RemoveDice"/>
        public void RemoveDice(int index) => source.RemoveDice(index);

        /// <inheritdoc cref="InteractiveRoller.RemoveDices"/>
        public void RemoveDices(IReadOnlySet<int> indices) => source.RemoveDices(indices);

        /// <inheritdoc cref="InteractiveRoller.Result"/>
        public Result Result() => source.Result();

        internal DiceRemoveStage(InteractiveRoller source) => this.source = source;
    }
}
