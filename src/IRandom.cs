namespace RP.ReverieWorld.DiceRoll;

/// <summary>
/// Provides an abstraction for (pseudo)random number generator for dice rollers.
/// </summary>
public interface IRandom : IDisposable
{
    /// <summary>
    /// Returns a non-negative random integer that is less than the specified maximum.
    /// </summary>
    /// <remarks>Same behavior as <see cref="System.Random.Next(int)"/> expected.</remarks>
    /// <param name="maxValue">The exclusive upper bound of the random number to be generated. <paramref name="maxValue"/> must be greater than or equal to 0.</param>
    /// <returns>A 32-bit signed integer that is greater than or equal to 0, and less than <paramref name="maxValue"/>.</returns>
    int Next(int maxValue);
}
