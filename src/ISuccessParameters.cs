namespace ReverieWorld.DiceRoll;

/// <summary>
/// Provides an abstraction for aggregation of requirements for success roll.
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
    /// Maximum dices count to roll. All dices exceeded threshold will be success.
    /// </summary>
    public int AutoSuccessThreshold { get; }
}
