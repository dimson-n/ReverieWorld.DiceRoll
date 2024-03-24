using ReverieWorld.DiceRoll.Modifiers;

namespace ReverieWorld.DiceRoll;

/// <summary>
/// Base implementation of the <see cref="IParameters"/> interface.
/// </summary>
public class ParametersBase : IParameters
{
    /// <summary>
    /// Named value to indicate infinity count of rerolls or bursts.
    /// </summary>
    public const int Infinite = -1;

    /// <inheritdoc/>
    public virtual int FacesCount { get; init; }

    /// <inheritdoc/>
    public virtual int DicesCount { get; init; }

    /// <inheritdoc/>
    public virtual int RerollsCount { get; init; }

    /// <inheritdoc/>
    public virtual int BurstsCount { get; init; }

    /// <inheritdoc/>
    public virtual int Bonus { get; init; }

    /// <summary>
    /// Gets value for indication that <see cref="RerollsCount"/> must be ignored and rerolls must be performed for all dices with value 1.
    /// </summary>
    /// <value><see langword="true"/> if <see cref="RerollsCount"/> must be ignored
    /// and rerolls must be performed for all dices with value 1; otherwise <see langword="false"/>.</value>
    public virtual bool HasInfinityRerolls => RerollsCount < 0;

    /// <summary>
    /// Gets value for indication that <see cref="BurstsCount"/> must be ignored and bursts must be performed for all dices with max possible value.
    /// </summary>
    /// <value><see langword="true"/> if <see cref="BurstsCount"/> must be ignored
    /// and burst must be performed for all dices with max possible value; otherwise <see langword="false"/>.</value>
    public virtual bool HasInfinityBursts => BurstsCount < 0;

    /// <inheritdoc/>
    public virtual IReadOnlyCollection<IRollModifier>? Modifiers { get; init; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ParametersBase"/> class with designated parameters for a dice roller.
    /// </summary>
    /// <param name="facesCount">Dice faces count.</param>
    /// <param name="dicesCount">Count of dices to roll.</param>
    /// <param name="rerollsCount">Count of possible rerolls for dices with value 1.</param>
    /// <param name="burstsCount">Count of possible bursts for dices with max possible value.</param>
    /// <param name="bonus">Bonus or penalty value for a roll.</param>
    /// <param name="modifiers">Modifiers for a roll.</param>
    public ParametersBase(int facesCount, int dicesCount = 1, int rerollsCount = 0, int burstsCount = 0,
                          int bonus = 0, IReadOnlyCollection<IRollModifier>? modifiers = null)
    {
        FacesCount = facesCount;
        DicesCount = dicesCount;
        RerollsCount = rerollsCount;
        BurstsCount = burstsCount;
        Bonus = bonus;
        Modifiers = modifiers;
    }
}
