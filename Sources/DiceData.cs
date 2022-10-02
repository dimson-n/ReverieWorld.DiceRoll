using System.Diagnostics;

namespace RP.ReverieWorld.DiceRoll
{

    [DebuggerDisplay("{Value}")]
    internal sealed class DiceData
    {
        public List<int> values;
        public bool removed;
        public bool burstMade;
        public bool isBurst;

        public int Value => values.Last();

        internal DiceData(int value, bool removed = false, bool burstMade = false, bool isBurst = false)
        {
            this.values = new List<int>(1) { value };
            this.removed = removed;
            this.burstMade = burstMade;
            this.isBurst = isBurst;
        }
    }
}
