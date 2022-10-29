namespace RP.ReverieWorld.DiceRoll;

/// <summary>
/// Represents an automatic dice roller.
/// </summary>
public sealed class AutoRoller
{
    private readonly IRandomProvider randomProvider;
    private readonly IParameters defaultParameters;
    private readonly IDiceRemoveStrategy diceRemoveStrategy;

    /// <summary>
    /// Initializes a new instance of the <see cref="AutoRoller"/> with specified <paramref name="randomProvider"/>,
    /// optional <paramref name="defaultParameters"/> and optional <paramref name="diceRemoveStrategy"/>.
    /// </summary>
    /// <param name="randomProvider">Implementation of <see cref="IRandomProvider"/> interface.</param>
    /// <param name="defaultParameters">Custom implementation of <see cref="IParameters"/> interface or <see cref="Parameters"/> (default).</param>
    /// <param name="diceRemoveStrategy">Custom implementation of <see cref="IDiceRemoveStrategy"/> interface or <see cref="DefaultDiceRemoveStrategy"/> (default).</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="randomProvider"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    /// <exception cref="ArgumentException"></exception>
    public AutoRoller(IRandomProvider randomProvider, IParameters? defaultParameters = null, IDiceRemoveStrategy? diceRemoveStrategy = null)
    {
        ArgumentNullException.ThrowIfNull(randomProvider);

        defaultParameters ??= Parameters.Default;
        Parameters.Validate(defaultParameters);

        this.randomProvider = randomProvider;
        this.defaultParameters = defaultParameters;
        this.diceRemoveStrategy = diceRemoveStrategy ?? new DefaultDiceRemoveStrategy();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AutoRoller"/> with specified <paramref name="randomProvider"/>
    /// and optional <paramref name="diceRemoveStrategy"/>.
    /// </summary>
    /// <param name="randomProvider">Implementation of <see cref="IRandomProvider"/> interface.</param>
    /// <param name="diceRemoveStrategy">Custom implementation of <see cref="IDiceRemoveStrategy"/> interface or <see cref="DefaultDiceRemoveStrategy"/> (default).</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="randomProvider"/> is <see langword="null"/>.</exception>
    public AutoRoller(IRandomProvider randomProvider, IDiceRemoveStrategy? diceRemoveStrategy) :
        this(randomProvider, null, diceRemoveStrategy)
    {
    }

    /// <summary>
    /// Performs the dice roll with optional <paramref name="parameters"/> and optional <paramref name="diceRemoveStrategy"/>.
    /// </summary>
    /// <remarks>If <paramref name="parameters"/> or <paramref name="diceRemoveStrategy"/> not provided default will be used.</remarks>
    /// <param name="parameters">Parameters for the roll.</param>
    /// <param name="diceRemoveStrategy">Dice selection strategy for an "add then remove" dice mechanic.</param>
    /// <returns>The <see cref="Result"/> of the dice roll.</returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    /// <exception cref="ArgumentException"></exception>
    public Result Roll(IParameters? parameters = null, IDiceRemoveStrategy? diceRemoveStrategy = null)
    {
        parameters ??= defaultParameters;
        Parameters.Validate(parameters);

        diceRemoveStrategy ??= this.diceRemoveStrategy;

        var current = new InteractiveRoller(randomProvider, parameters).Begin();

        if (parameters.AdditionalDicesCount != 0)
        {
            current.RemoveDices(diceRemoveStrategy.Select(current.Values, parameters.AdditionalDicesCount, parameters));
        }

        return current.Result();
    }

    /// <summary>
    /// Performs the dice roll with default parameters and optional <paramref name="diceRemoveStrategy"/>.
    /// </summary>
    /// <remarks>If <paramref name="diceRemoveStrategy"/> not provided the default will be used.</remarks>
    /// <param name="diceRemoveStrategy"></param>
    /// <returns>The <see cref="Result"/> of the dice roll.</returns>
    public Result Roll(IDiceRemoveStrategy? diceRemoveStrategy)
    {
        return Roll(null, diceRemoveStrategy);
    }
}
