using System.Collections;

namespace ReverieWorld.DiceRoll;

/// <summary>
/// Represents current state of the dice roll.
/// </summary>
public class Roll : IReadOnlyList<Dice>
{
    private readonly IReadOnlyList<Dice> rolls;

    /// <summary>
    /// Gets parameters of the <see cref="Roll"/>.
    /// </summary>
    /// <value>Parameters of the <see cref="Roll"/>.</value>
    public IParameters Parameters { get; }

    /// <summary>
    /// Total summation of all dice values with bonus.
    /// </summary>
    public int Total { get; }

    /// <summary>
    /// Gets a value indicating whether <see cref="Roll"/> was fully performed.
    /// </summary>
    /// <value><see langword="true"/> if <see cref="Roll"/> was fully performed; otherwise, <see langword="false"/>.</value>
    public virtual bool Completed => false;

    internal Roll(RollState state)
    {
        rolls = state.Values;
        Parameters = state.parameters;

        Total = rolls.Where(d => !d.Removed).Sum(d => d.Value) + Parameters.Bonus;
    }

    /// <summary>
    /// Gets the <see cref="Dice"/> at the specified index in the <see cref="Roll"/>.
    /// </summary>
    /// <param name="index">The zero-based index of the <see cref="Dice"/> to get.</param>
    /// <value>The <see cref="Dice"/> at the specified index in the <see cref="Roll"/>.</value>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public Dice this[int index] => rolls[index];

    /// <summary>
    /// Gets the number of <see cref="Dice"/>s in the <see cref="Roll"/>.
    /// </summary>
    /// <value>The number of <see cref="Dice"/>s in the <see cref="Roll"/>.</value>
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
    public override sealed string ToString() => Total.ToString();
}
