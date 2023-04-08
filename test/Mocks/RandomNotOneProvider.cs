namespace ReverieWorld.DiceRoll.Tests;

internal sealed class RandomNotOneProvider : IRandomProvider
{
    public IRandom Lock() => new RandomImpl();

    private sealed class RandomImpl : IRandom
    {
        private readonly Random rand = Random.Shared;

        public int Next(int maxValue) => rand.Next(maxValue - 1) + 1;

        public void Dispose() { }
    }
}
