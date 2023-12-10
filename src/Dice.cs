using System.Collections;

namespace ReverieWorld.DiceRoll;

/// <summary>
/// Represents one dice in a <see cref="Roll"/>.
/// </summary>
public sealed class Dice : IReadOnlyList<int>
{
    private readonly List<int> values;
    internal bool burstMade = false;

    /// <summary>
    /// Gets actual value of the <see cref="Dice"/>.
    /// </summary>
    /// <value>Actual value of the <see cref="Dice"/>.</value>
    public int Value
    {
        get => values[^1];
        internal set => values.Add(value);
    }

    /// <summary>
    /// Gets <see cref="Dice"/> generation offset (<see cref="Roll"/> round).
    /// </summary>
    /// <value><see cref="Dice"/> generation offset (<see cref="Roll"/> round).</value>
    public int Offset { get; }

    /// <summary>
    /// Gets the number of rolls of the <see cref="Dice"/>.
    /// </summary>
    /// <value>The number of rolls of the <see cref="Dice"/>.</value>
    public int RollsCount => values.Count;

    /// <summary>
    /// Gets a value indicating whether the <see cref="Dice"/> excluded from roll result.
    /// </summary>
    /// <value><see langword="true"/> if the <see cref="Dice"/> was excluded from roll result; otherwise, <see langword="false"/>.</value>
    public bool Removed { get; internal set; } = false;

    /// <summary>
    /// Gets a value indicating whether the <see cref="Dice"/> made as burst.
    /// </summary>
    /// <value><see langword="true"/> if the <see cref="Dice"/> was made as burst; otherwise, <see langword="false"/>.</value>
    public bool IsBurst { get; }

    /// <summary>
    /// Gets a value indicating whether the <see cref="Dice"/> was modified by some <see cref="Modifiers.IRollModifier"/>.
    /// </summary>
    /// <value><see langword="true"/> if the <see cref="Dice"/> was modified by some <see cref="Modifiers.IRollModifier"/>; otherwise, <see langword="false"/>.</value>
    public bool Modified { get; internal set; }

    internal Dice(int value, int offset = 0, bool isBurst = false, bool fromModifier = false)
    {
        this.values   = new List<int>(1) { value };
        this.Offset   = offset;
        this.IsBurst  = isBurst;
        this.Modified = fromModifier;
    }

    /// <summary>
    /// Gets the dice roll value at the specified index.
    /// </summary>
    /// <param name="index">The zero-based index of the dice roll to get.</param>
    /// <value>The dice roll value at the specified index.</value>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public int this[int index] => values[index];

    int IReadOnlyCollection<int>.Count => values.Count;

    /// <summary>
    /// Returns an enumerator that iterates through the collection of dice rolls.
    /// </summary>
    /// <returns>An enumerator for <see cref="IReadOnlyList{Int32}"/>.</returns>
    public IEnumerator<int> GetEnumerator() => values.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)values).GetEnumerator();

    /// <summary>
    /// Returns a string that represents the <see cref="Dice"/>.
    /// </summary>
    /// <returns>A string that represents the <see cref="Dice"/>.</returns>
    public override string ToString()
    {
        return $"{(Removed ? '-' : string.Empty)}{(IsBurst ? '*' : string.Empty)}{Value}{(Modified ? '\'' : string.Empty)}";
    }
}
