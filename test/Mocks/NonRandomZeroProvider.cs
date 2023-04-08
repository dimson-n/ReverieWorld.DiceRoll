namespace ReverieWorld.DiceRoll.Tests;

internal sealed class NonRandomZeroProvider : IRandomProvider
{
    public IRandom Lock() => new Zero();

    private sealed class Zero : IRandom
    {
        public int Next(int maxValue) => 0;

        public void Dispose() { }
    }
}
