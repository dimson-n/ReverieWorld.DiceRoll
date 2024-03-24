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
        Ready,
        Completed,
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
#pragma warning disable CS0618
    public DiceRemoveStage Begin()
    {
        if (stage != Stage.Init)
        {
            throw new InvalidOperationException("Begin can be called once after initialization only");
        }

        state.FillInitial();

        stage = Stage.Ready;

        return new DiceRemoveStage(this);
    }
#pragma warning restore CS0618

    /// <summary>
    /// Completes the dice roll interaction.
    /// </summary>
    /// <returns><see cref="DiceRoll.Result"/> of the dice roll.</returns>
    /// <exception cref="InvalidOperationException"></exception>
    public Result Result()
    {
        if (stage != Stage.Ready)
        {
            throw new InvalidOperationException("Can't get result at current stage");
        }

        state.CompleteRerollsAndBursts();

        result = new Result(state);

        stage = Stage.Completed;

        return result;
    }

    /// <summary>
    /// Represents a dice remove stage of interactive roll.
    /// </summary>
    [Obsolete("Not longer in use, will be replaced with another stage")]
    public sealed class DiceRemoveStage
    {
        private readonly InteractiveRoller source;

        /// <inheritdoc cref="InteractiveRoller.Values"/>
        public IReadOnlyList<Dice> Values => source.Values;

        /// <inheritdoc cref="InteractiveRoller.Current"/>
        public Roll Current => source.Current;

        /// <summary>
        /// Indicates that proper count of <see cref="Dice"/>s already removed from the <see cref="Roll"/>.
        /// </summary>
        /// <value><see langword="true"/> if there is no <see cref="Dice"/>s to remove; otherwise <see langword="false"/>.</value>
        public bool StageConditionsMet => true;

        /// <inheritdoc cref="InteractiveRoller.Result"/>
        public Result Result() => source.Result();

        internal DiceRemoveStage(InteractiveRoller source) => this.source = source;
    }
}
