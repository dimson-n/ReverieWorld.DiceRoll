namespace ReverieWorld.DiceRoll;

/// <summary>
/// Provides an abstraction for aggregation of parameters for success roll.
/// </summary>
public interface ISuccessParameters
{
    /// <summary>
    /// Minimal dice value to be success.
    /// </summary>
    int MinValue { get; }

    /// <summary>
    /// Minimal succeed dices to roll success.
    /// </summary>
    int Count { get; }

    /// <summary>
    /// Maximum dices count to roll. All dices exceeded threshold automatically will be success.
    /// </summary>
    int AutoSuccessThreshold { get; }
}
