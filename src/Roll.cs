using System.Collections;

namespace RP.ReverieWorld.DiceRoll;

/// <summary>
/// Represents current state of the dice roll.
/// </summary>
public class Roll : IReadOnlyList<Dice>
{
    private readonly IReadOnlyList<Dice> rolls;

    private readonly IParameters parameters;

    /// <summary>
    /// Total summation of all dice values and <see cref="Bonus"/>.
    /// </summary>
    public int Total { get; }

    /// <summary>
    /// Bonus (positive) or penalty (negative) value that added to final summation of dice values.
    /// </summary>
    public int Bonus => parameters.Bonus;

    /// <summary>
    /// Dice faces count.
    /// </summary>
    public int DiceFacesCount => parameters.FacesCount;

    /// <summary>
    /// Initial count of dices to roll.
    /// </summary>
    public int BaseDicesCount => parameters.DicesCount;

    /// <summary>
    /// Count of dices for "add then remove" dice mechanic.
    /// </summary>
    public int AdditionalDicesCount => parameters.AdditionalDicesCount;

    /// <summary>
    /// Count of possible rerolls for dices with value 1.
    /// </summary>
    public int InitialRerollsCount => parameters.RerollsCount;

    /// <summary>
    /// Count of possible bursts for dices with max possible value.
    /// </summary>
    public int InitialBurstsCount => parameters.BurstsCount;

    /// <summary>
    /// Value that indicates that <see cref="InitialRerollsCount"/> ignored and rerolls must be performed for all dices with value 1.
    /// </summary>
    public bool HasInfinityRerolls => parameters.HasInfinityRerolls;

    /// <summary>
    /// Value that indicates that <see cref="InitialBurstsCount"/> ignored and burtst must be performed for all dices with max possible value.
    /// </summary>
    public bool HasInfinityBursts => parameters.HasInfinityBursts;

    /// <summary>
    /// Gets a value indicating whether <see cref="Roll"/> was fully performed.
    /// </summary>
    /// <returns><see langword="true"/> if <see cref="Roll"/> was fully performed; otherwise, <see langword="false"/>.</returns>
    public virtual bool Completed => false;

    internal Roll(IReadOnlyList<Dice> rolls, IParameters parameters)
    {
        this.rolls = rolls;
        this.parameters = parameters;

        Total = rolls.Where(d => !d.Removed).Sum(d => d.Value) + Bonus;
    }

    /// <summary>
    /// Gets the <see cref="Dice"/> at the specified index in the <see cref="Roll"/>.
    /// </summary>
    /// <param name="index">The zero-based index of the <see cref="Dice"/> to get.</param>
    /// <returns>The <see cref="Dice"/> at the specified index in the <see cref="Roll"/>.</returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public Dice this[int index] => rolls[index];

    /// <summary>
    /// Gets the number of <see cref="Dice"/>s in the <see cref="Roll"/>.
    /// </summary>
    /// <returns>The number of <see cref="Dice"/>s in the <see cref="Roll"/>.</returns>
    public int Count => rolls.Count;

    /// <summary>
    /// Returns an enumerator that iterates through the collection of <see cref="Dice"/>s.
    /// </summary>
    /// <returns>An enumerator for <see cref="IReadOnlyList{Dice}"/>.</returns>
    public IEnumerator<Dice> GetEnumerator() => rolls.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)rolls).GetEnumerator();

    /// <summary>
    /// Returns a string that represents the <see cref="Roll"/> value.
    /// </summary>
    /// <returns>A string that represents the <see cref="Roll"/> value.</returns>
    public override string ToString()
    {
        return Total.ToString();
    }
}
