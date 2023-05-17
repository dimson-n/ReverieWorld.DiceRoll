using ReverieWorld.DiceRoll.Modifiers;

namespace ReverieWorld.DiceRoll;

/// <summary>
/// Default implementation of the <see cref="IParameters"/> interface with preset by d6 dices.
/// </summary>
public class Parameters : GenericParameters
{
    internal static readonly Parameters Default = new();

    /// <summary>
    /// Predefined value for <see cref="GenericParameters.FacesCount"/> property.
    /// </summary>
    public const int DiceFacesCount = 6;

    /// <summary>
    /// Initializes a new instance of the <see cref="Parameters"/> class with designated parameters for a dice roller.
    /// </summary>
    /// <param name="dicesCount">Count of dices to roll.</param>
    /// <param name="additionalDicesCount">Count of dices for "add then remove" dice mechanic.</param>
    /// <param name="rerollsCount">Count of possible rerolls for dices with value 1.</param>
    /// <param name="burstsCount">Count of possible bursts for dices with max possible value.</param>
    /// <param name="bonus">Bonus or penalty value for a roll.</param>
    /// <param name="modifiers">Optional modifiers for a roll.</param>
    public Parameters(int dicesCount = 1, int additionalDicesCount = 0, int rerollsCount = 0,
                      int burstsCount = 0, int bonus = 0, IReadOnlyCollection<IRollModifier>? modifiers = null) :
        base(DiceFacesCount, dicesCount, additionalDicesCount, rerollsCount, burstsCount, bonus, modifiers)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Parameters"/> class with designated parameters for a dice roller.
    /// </summary>
    /// <param name="modifier">Optional modifier for a roll.</param>
    /// <param name="dicesCount">Count of dices to roll.</param>
    /// <param name="additionalDicesCount">Count of dices for "add then remove" dice mechanic.</param>
    /// <param name="rerollsCount">Count of possible rerolls for dices with value 1.</param>
    /// <param name="burstsCount">Count of possible bursts for dices with max possible value.</param>
    /// <param name="bonus">Bonus or penalty value for a roll.</param>
    public Parameters(IRollModifier? modifier, int dicesCount = 1, int additionalDicesCount = 0,
                      int rerollsCount = 0, int burstsCount = 0, int bonus = 0) :
        this(dicesCount, additionalDicesCount, rerollsCount, burstsCount, bonus,
             modifier is null ? null : new[] { modifier })
    {
    }
}
