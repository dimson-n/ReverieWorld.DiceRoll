namespace RP.ReverieWorld.DiceRoll.Tests;

internal sealed class NonRandomMaxProvider : IRandomProvider
{
    public IRandom Lock() => new Max();

    private sealed class Max : IRandom
    {
        public int Next(int maxValue) => Math.Max(0, maxValue - 1);

        public void Dispose() { }
    }
}
