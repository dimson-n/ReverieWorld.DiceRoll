namespace ReverieWorld.DiceRoll;

/// <summary>
/// Non thread-safe implementation of PRNG for dice rollers.
/// </summary>
/// <remarks>Faster than default but not thread-safe.</remarks>
/// <seealso cref="DefaultRandomProvider"/>
public sealed class SingleThreadedRandomProvider : IRandomProvider, IRandom
{
    private readonly Random rnd = new();

    /// <summary>
    /// Gets a thread-safe instance of PRNG for dice rollers.
    /// </summary>
    /// <returns>A thread-safe instance of PRNG for dice rollers.</returns>
    public IRandom Lock() => this;

    int IRandom.Next(int maxValue) => rnd.Next(maxValue);

    void IDisposable.Dispose() { }
}
