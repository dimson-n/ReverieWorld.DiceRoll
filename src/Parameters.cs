using ReverieWorld.DiceRoll.Modifiers;

namespace ReverieWorld.DiceRoll;

/// <summary>
/// Default implementation of the <see cref="IParameters"/> interface.
/// </summary>
public class Parameters : IParameters
{
    /// <summary>
    /// Default parameters.
    /// </summary>
    public static readonly Parameters Default = new();

    /// <summary>
    /// Default value for <see cref="FacesCount"/> property.
    /// </summary>
    public const int DefaultFacesCount = 6;

    /// <summary>
    /// Named value to indicate infinity count of rerolls or bursts.
    /// </summary>
    public const int Infinite = -1;

    /// <inheritdoc/>
    public int FacesCount { get; init; }

    /// <inheritdoc/>
    public int DicesCount { get; init; }

    /// <inheritdoc/>
    public int RerollsCount { get; init; }

    /// <inheritdoc/>
    public int BurstsCount { get; init; }

    /// <inheritdoc/>
    public int Efficiency { get; init; }

    /// <inheritdoc/>
    public int AutoSuccesses { get; init; }

    /// <summary>
    /// Gets value for indication that <see cref="RerollsCount"/> must be ignored and rerolls must be performed for all dices with value 1.
    /// </summary>
    /// <value><see langword="true"/> if <see cref="RerollsCount"/> must be ignored
    /// and rerolls must be performed for all dices with value 1; otherwise <see langword="false"/>.</value>
    public bool HasInfinityRerolls => RerollsCount < 0;

    /// <summary>
    /// Gets value for indication that <see cref="BurstsCount"/> must be ignored and bursts must be performed for all dices with max possible value.
    /// </summary>
    /// <value><see langword="true"/> if <see cref="BurstsCount"/> must be ignored
    /// and burst must be performed for all dices with max possible value; otherwise <see langword="false"/>.</value>
    public bool HasInfinityBursts => BurstsCount < 0;

    /// <inheritdoc/>
    public IReadOnlyCollection<IRollModifier>? Modifiers { get; init; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Parameters"/> class with designated parameters for a dice roller.
    /// </summary>
    /// <param name="facesCount">Dice faces count.</param>
    /// <param name="dicesCount">Count of dices to roll.</param>
    /// <param name="rerollsCount">Count of possible rerolls for dices with value 1.</param>
    /// <param name="burstsCount">Count of possible bursts for dices with max possible value.</param>
    /// <param name="efficiency">Efficiency value for a roll.</param>
    /// <param name="autoSuccesses">Count of guaranteed successes in a roll.</param>
    /// <param name="modifiers">Modifiers for a roll.</param>
    public Parameters(int facesCount = DefaultFacesCount, int dicesCount = 1, int rerollsCount = 0, int burstsCount = 0,
                      int efficiency = 0, int autoSuccesses = 0, IReadOnlyCollection<IRollModifier>? modifiers = null)
    {
        FacesCount = facesCount;
        DicesCount = dicesCount;
        RerollsCount = rerollsCount;
        BurstsCount = burstsCount;
        Efficiency = efficiency;
        AutoSuccesses = autoSuccesses;
        Modifiers = modifiers;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Parameters"/> class with designated parameters for a dice roller.
    /// </summary>
    /// <param name="modifier">Optional modifier for a roll.</param>
    /// <param name="facesCount">Dice faces count.</param>
    /// <param name="dicesCount">Count of dices to roll.</param>
    /// <param name="rerollsCount">Count of possible rerolls for dices with value 1.</param>
    /// <param name="burstsCount">Count of possible bursts for dices with max possible value.</param>
    /// <param name="efficiency">Efficiency value for a roll.</param>
    /// <param name="autoSuccesses">Count of guaranteed successes in a roll.</param>
    public Parameters(IRollModifier? modifier, int facesCount = DefaultFacesCount, int dicesCount = 1,
                      int rerollsCount = 0, int burstsCount = 0, int efficiency = 0, int autoSuccesses = 0) :
        this(facesCount, dicesCount, rerollsCount, burstsCount, efficiency, autoSuccesses, modifier is null ? null : new[] { modifier })
    {
    }
}
