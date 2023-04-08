namespace ReverieWorld.DiceRoll;

/// <summary>
/// Provides an abstraction for (pseudo)random number generator provider for dice rollers.
/// </summary>
public interface IRandomProvider
{
    /// <summary>
    /// Gets an instance of (pseudo)random number generator for dice rollers.
    /// </summary>
    /// <returns>An instance of (pseudo)random number generator for dice rollers.</returns>
    IRandom Lock();
}
