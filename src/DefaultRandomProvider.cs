namespace RP.ReverieWorld.DiceRoll;

/// <summary>
/// Default thread-safe implementation of PRNG for dice rollers.
/// </summary>
/// <remarks>Recomended to implement it in you own way.</remarks>
public sealed class DefaultRandomProvider : IRandomProvider
{
    /// <summary>
    /// Gets a thread-safe instance of PRNG for dice rollers.
    /// </summary>
    /// <returns>A thread-safe instance of PRNG for dice rollers.</returns>
    public IRandom Lock() => new RandomImpl();

    private sealed class RandomImpl : IRandom
    {
        private readonly Random rnd = Random.Shared;

        public int Next(int maxValue) => rnd.Next(maxValue);

        public void Dispose() { }
    }
}
