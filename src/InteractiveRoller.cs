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
        EfficiencyDistribution,
        Completed,
    }

    private Stage stage = Stage.Init;

    private readonly RollState _state;

    private Result? result;

    /// <summary>
    /// Gets a read-only list of dices of current state.
    /// </summary>
    /// <value>A <see cref="IReadOnlyList{T}"/> of <see cref="Dice"/>s.</value>
    public IReadOnlyList<Dice> Values
        => _state.Values;

    /// <summary>
    /// Gets current <see cref="Roll"/> state.
    /// </summary>
    /// <value>Current <see cref="Roll"/> state.</value>
    public Roll Current
        => result ?? new Roll(_state);

    /// <summary>
    /// Indicates that efficiency can be distributed to some <see cref="Dice"/>.
    /// </summary>
    /// <value><see langword="true"/> if efficiency can be added to some <see cref="Dice"/>; otherwise <see langword="false"/>.</value>
    public bool CanDistributeEfficiency
    {
        get
        {
            var maxValue = _state.Parameters.FacesCount;
            return _state.RemainingEfficiency > 0 && _state.Any(d => d.Value != maxValue);
        }
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="InteractiveRoller"/> with specified <paramref name="randomProvider"/> and optional <paramref name="parameters"/>.
    /// </summary>
    /// <param name="randomProvider">Implementation of <see cref="IRandomProvider"/> interface.</param>
    /// <param name="efficiencyDistributionStrategy">An implementation of efficiency distribution strategy.</param>
    /// <param name="parameters">Custom implementation of <see cref="IParameters"/> interface or <see cref="Parameters"/> (default).</param>
    /// <param name="successParameters">Parameters for success roll.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="randomProvider"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    /// <exception cref="ArgumentException"></exception>
    public InteractiveRoller(IRandomProvider randomProvider, IEfficiencyDistributionStrategy? efficiencyDistributionStrategy = null, IParameters? parameters = null, ISuccessParameters? successParameters = null)
    {
        ArgumentNullException.ThrowIfNull(randomProvider);

        parameters ??= Parameters.Default;
        parameters.Validate();

        if (successParameters is not null)
        {
            successParameters.Validate();
            parameters.ValidateApplicability(successParameters);
        }

        efficiencyDistributionStrategy ??= new DefaultEfficiencyDistributionStrategy();

        _state = new(randomProvider, efficiencyDistributionStrategy, parameters, successParameters);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="InteractiveRoller"/> with default efficiency distribution strategy.
    /// </summary>
    /// <param name="randomProvider">Implementation of <see cref="IRandomProvider"/> interface.</param>
    /// <param name="parameters">Custom implementation of <see cref="IParameters"/> interface or <see cref="Parameters"/> (default).</param>
    /// <param name="successParameters">Parameters for success roll.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="randomProvider"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    /// <exception cref="ArgumentException"></exception>
    public InteractiveRoller(IRandomProvider randomProvider, IParameters? parameters, ISuccessParameters? successParameters = null) :
        this(randomProvider, null, parameters, successParameters)
    {
    }

    /// <summary>
    /// Performs initial dice roll.
    /// </summary>
    /// <returns>Next stage wrapper.</returns>
    /// <exception cref="InvalidOperationException"></exception>
    public EfficiencyDistributionStage Begin()
    {
        if (stage != Stage.Init)
        {
            throw new InvalidOperationException("Begin can be called once after initialization only");
        }

        _state.FillInitial();

        stage = Stage.EfficiencyDistribution;

        return new EfficiencyDistributionStage(this);
    }

    /// <summary>
    /// Adds efficiency to dice with designated index.
    /// </summary>
    /// <param name="diceIndex">Dice index to add efficiency.</param>
    /// <param name="value">Efficiency to add.</param>
    /// <returns>Added efficiency.</returns>
    /// <exception cref="ArgumentOutOfRangeException" />
    /// <exception cref="InvalidOperationException" />
    public int AddEfficiency(int diceIndex, int value = 1)
    {
        if (stage != Stage.EfficiencyDistribution)
        {
            throw new InvalidOperationException("Can not add efficiency at current stage");
        }

        return _state.AddEfficiency(diceIndex, value);
    }

    /// <summary>
    /// Adds efficiency to designated dice.
    /// </summary>
    /// <param name="dice">Dice to add efficiency.</param>
    /// <param name="value">Efficiency to add.</param>
    /// <returns>Added efficiency.</returns>
    /// <exception cref="ArgumentOutOfRangeException" />
    /// <exception cref="InvalidOperationException" />
    public int AddEfficiency(Dice dice, int value = 1)
    {
        if (stage != Stage.EfficiencyDistribution)
        {
            throw new InvalidOperationException("Can not add efficiency at current stage");
        }

        return _state.AddEfficiency(dice, value);
    }

    /// <summary>
    /// Completes the dice roll interaction.
    /// </summary>
    /// <returns><see cref="DiceRoll.Result"/> of the dice roll.</returns>
    /// <exception cref="InvalidOperationException"></exception>
    public Result Result()
    {
        if (stage != Stage.EfficiencyDistribution)
        {
            throw new InvalidOperationException("Can not get result at current stage");
        }

        _state.MakeRerollsAndBursts();

        result = new Result(_state);

        stage = Stage.Completed;

        return result;
    }

    /// <summary>
    /// Represents an efficiency distribution stage of interactive roll.
    /// </summary>
    public sealed class EfficiencyDistributionStage
    {
        private readonly InteractiveRoller _source;

        /// <inheritdoc cref="InteractiveRoller.Values"/>
        public IReadOnlyList<Dice> Values
            => _source.Values;

        /// <inheritdoc cref="InteractiveRoller.Current"/>
        public Roll Current
            => _source.Current;

        /// <inheritdoc cref="InteractiveRoller.AddEfficiency(int, int)"/>
        public int AddEfficiency(int diceIndex, int value = 1)
            => _source.AddEfficiency(diceIndex, value);

        /// <inheritdoc cref="InteractiveRoller.AddEfficiency(Dice, int)"/>
        public int AddEfficiency(Dice dice, int value = 1)
            => _source.AddEfficiency(dice, value);

        /// <summary>
        /// Indicates that all possible efficiency distributed between <see cref="Dice"/>s.
        /// </summary>
        /// <value><see langword="true"/> if no efficiency can be added to any <see cref="Dice"/>; otherwise <see langword="false"/>.</value>
        public bool ConditionsMet
            => !_source.CanDistributeEfficiency;

        /// <inheritdoc cref="InteractiveRoller.Result"/>
        public Result Result()
            => _source.Result();

        internal EfficiencyDistributionStage(InteractiveRoller source)
            => _source = source;
    }
}
