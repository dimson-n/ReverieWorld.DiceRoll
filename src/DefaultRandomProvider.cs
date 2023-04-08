namespace RP.ReverieWorld.DiceRoll;

/// <summary>
/// Default thread-safe implementation of PRNG for dice rollers.
/// </summary>
/// <remarks>Recommended to implement it in you own way.</remarks>
public sealed class DefaultRandomProvider : IRandomProvider, IRandom
{
    private readonly Random rnd = Random.Shared;

    /// <summary>
    /// Gets a thread-safe instance of PRNG for dice rollers.
    /// </summary>
    /// <returns>A thread-safe instance of PRNG for dice rollers.</returns>
    public IRandom Lock() => this;

    int IRandom.Next(int maxValue) => rnd.Next(maxValue);

    void IDisposable.Dispose() { }
}
