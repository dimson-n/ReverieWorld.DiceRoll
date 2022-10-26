namespace RP.ReverieWorld.DiceRoll;

/// <summary>
/// Provides an abstraction of a parameters group for dice rollers.
/// </summary>
public interface IParameters
{
    /// <summary>
    /// Dice faces count.
    /// </summary>
    int FacesCount { get; }

    /// <summary>
    /// Count of dices to roll.
    /// </summary>
    int DicesCount { get; }

    /// <summary>
    /// Count of additional dices that will be added to initial roll.
    /// Than same count of dices will be removed (not necessary the same dices).
    /// </summary>
    int AdditionalDicesCount { get; }

    /// <summary>
    /// Count of possible rerolls for dices with value 1.
    /// </summary>
    int RerollsCount { get; }

    /// <summary>
    /// Count of possible bursts for dices with max possible value.
    /// </summary>
    int BurstsCount { get; }

    /// <summary>
    /// Bonus (positive) or penalty (negative) value that will be added to final summation of dice values.
    /// </summary>
    int Bonus { get; }

    /// <summary>
    /// <see langword="true"/> indicates that <see cref="RerollsCount"/> must be ignored
    /// and rerolls must be performed for all dices with value 1; otherwise <see langword="false"/>.
    /// </summary>
    bool HasInfinityRerolls => RerollsCount < 0;

    /// <summary>
    /// <see langword="true"/> indicates that <see cref="BurstsCount"/> must be ignored
    /// and burtst must be performed for all dices with max possible value; otherwise <see langword="false"/>.
    /// </summary>
    bool HasInfinityBursts => BurstsCount < 0;
}
