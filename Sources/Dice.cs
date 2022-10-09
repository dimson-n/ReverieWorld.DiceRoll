using System.Collections;

namespace RP.ReverieWorld.DiceRoll
{
    /// <summary>
    /// Represents one dice in roll.
    /// </summary>
    public sealed class Dice : IReadOnlyList<int>
    {
        internal readonly List<int> values;
        internal bool burstMade;

        /// <summary>
        /// Actual value of dice.
        /// </summary>
        public int Value => values.Last();

        public int RollsCount => values.Count;

        /// <returns><see langword="true"/> if dice was excluded from roll result; otherwise, <see langword="false"/>.</returns>
        public bool Removed { get; internal set; }

        public bool IsBurst { get; internal set; }

        internal Dice(int value, bool removed = false, bool burstMade = false, bool isBurst = false)
        {
            this.values = new List<int>(1) { value };
            this.Removed = removed;
            this.burstMade = burstMade;
            this.IsBurst = isBurst;
        }

        public int this[int index] => values[index];

        int IReadOnlyCollection<int>.Count => values.Count;

        public IEnumerator<int> GetEnumerator() => values.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)values).GetEnumerator();

        public override string ToString()
        {
            return $"{(Removed ? '-' : string.Empty)}{(IsBurst ? '*' : string.Empty)}{Value}";
        }
    }
}
