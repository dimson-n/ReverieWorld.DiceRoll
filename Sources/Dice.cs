using System.Collections;

namespace RP.ReverieWorld.DiceRoll
{
    /// <summary>
    /// Represents one dice in roll result.
    /// </summary>
    public sealed class Dice : IReadOnlyList<int>
    {
        private readonly DiceData data;

        public int Value { get; }
        public int RollsCount => data.values.Count;
        public bool WasRemoved => data.removed;
        public bool IsBurst => data.isBurst;

        internal Dice(DiceData data)
        {
            this.data = data;

            Value = data.Value;
        }

        public int this[int index] => data.values[index];

        public int Count => data.values.Count;

        public IEnumerator<int> GetEnumerator() => data.values.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)data.values).GetEnumerator();

        public override string ToString()
        {
            return $"{(WasRemoved ? '-' : string.Empty)}{(IsBurst ? '*' : string.Empty)}{Value}";
        }
    }
}
