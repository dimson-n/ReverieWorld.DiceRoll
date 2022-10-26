using System.Collections;

namespace RP.ReverieWorld.DiceRoll
{
    /// <summary>
    /// Represents one dice in the <see cref="Roll"/>.
    /// </summary>
    public sealed class Dice : IReadOnlyList<int>
    {
        internal readonly List<int> values;
        internal bool burstMade;

        /// <summary>
        /// Gets actual value of the <see cref="Dice"/>.
        /// </summary>
        /// <returns>Actual value of the <see cref="Dice"/>.</returns>
        public int Value => values.Last();

        /// <summary>
        /// Gets <see cref="Dice"/> generation offset (<see cref="Roll"/> round).
        /// </summary>
        /// <returns><see cref="Dice"/> generation offset (<see cref="Roll"/> round).</returns>
        public int Offset { get; }

        /// <summary>
        /// Gets the number of rolls of the <see cref="Dice"/>.
        /// </summary>
        /// <returns>The number of rolls of the <see cref="Dice"/>.</returns>
        public int RollsCount => values.Count;

        /// <summary>
        /// Gets a value indicating whether the <see cref="Dice"/> excluded from roll result.
        /// </summary>
        /// <returns><see langword="true"/> if the <see cref="Dice"/> was excluded from roll result; otherwise, <see langword="false"/>.</returns>
        public bool Removed { get; internal set; }

        /// <summary>
        /// Gets a value indicating whether the <see cref="Dice"/> made as burst.
        /// </summary>
        /// <returns><see langword="true"/> if the <see cref="Dice"/> was made as burst; otherwise, <see langword="false"/>.</returns>
        public bool IsBurst { get; }

        internal Dice(int value, int offset = 0, bool isBurst = false)
        {
            this.values    = new List<int>(1) { value };
            this.Offset    = offset;
            this.burstMade = false;
            this.Removed   = false;
            this.IsBurst   = isBurst;
        }

        /// <summary>
        /// Gets the dice roll value at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the dice roll to get.</param>
        /// <returns>The dice roll value at the specified index.</returns>
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
            return $"{(Removed ? '-' : string.Empty)}{(IsBurst ? '*' : string.Empty)}{Value}";
        }
    }
}
