using ReverieWorld.DiceRoll.Utils;

namespace ReverieWorld.DiceRoll;

/// <summary>
/// Represents an automatic dice roller.
/// </summary>
/// <seealso cref="InteractiveRoller"/>
public sealed class AutoRoller
{
    private readonly IRandomProvider randomProvider;
    private readonly IEfficiencyDistributionStrategy _efficiencyDistributionStrategy;
    private readonly IParameters defaultParameters;

    /// <summary>
    /// Initializes a new instance of the <see cref="AutoRoller"/> with specified <paramref name="randomProvider"/>
    /// and optional <paramref name="defaultParameters"/>.
    /// </summary>
    /// <param name="randomProvider">Implementation of <see cref="IRandomProvider"/> interface.</param>
    /// <param name="efficiencyDistributionStrategy">An implementation of efficiency distribution strategy.</param>
    /// <param name="defaultParameters">Custom implementation of <see cref="IParameters"/> interface or <see cref="Parameters"/> (default).</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="randomProvider"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    /// <exception cref="ArgumentException"></exception>
    public AutoRoller(IRandomProvider randomProvider, IEfficiencyDistributionStrategy? efficiencyDistributionStrategy = null, IParameters? defaultParameters = null)
    {
        ArgumentNullException.ThrowIfNull(randomProvider);

        defaultParameters ??= Parameters.Default;
        defaultParameters.Validate();

        this.randomProvider = randomProvider;
        _efficiencyDistributionStrategy = efficiencyDistributionStrategy ?? new DefaultEfficiencyDistributionStrategy();
        this.defaultParameters = defaultParameters;
    }

    /// <summary>
    /// Performs the dice roll with optional <paramref name="parameters"/>.
    /// </summary>
    /// <remarks>If <paramref name="parameters"/> not provided a default will be used.</remarks>
    /// <param name="parameters">Parameters for the roll.</param>
    /// <param name="successParameters">Success parameters for the roll.</param>
    /// <returns>The <see cref="Result"/> of the dice roll.</returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    /// <exception cref="ArgumentException"></exception>
    public Result Roll(IParameters? parameters, ISuccessParameters successParameters)
    {
        parameters ??= defaultParameters;
        parameters.Validate();

        successParameters.Validate();
        parameters.ValidateApplicability(successParameters);

        RollState roll = new(randomProvider, _efficiencyDistributionStrategy, parameters, successParameters);

        using (RollMaker rollMaker = new(roll))
        {
            roll.FillInitial(rollMaker);

            for (bool loop = true; loop;)
            {
                roll.MakeRerollsAndBursts(rollMaker);
                loop = roll.DistributeEfficiency();
            }
        }

        return new Result(roll);
    }

    /// <summary>
    /// Performs the dice roll with default parameters.
    /// </summary>
    /// <param name="successParameters">Success parameters for the roll.</param>
    /// <returns>The <see cref="Result"/> of the dice roll.</returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    /// <exception cref="ArgumentException"></exception>
    public Result Roll(ISuccessParameters successParameters)
        => Roll(null, successParameters);
}
