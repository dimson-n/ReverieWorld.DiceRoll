namespace ReverieWorld.DiceRoll;

/// <summary>
/// Provides an abstraction for aggregation of requirements for success roll.
/// </summary>
public interface ISuccessParameters
{
    /// <summary>
    /// Minimal dice value to success.
    /// </summary>
    int MinValue { get; }

    /// <summary>
    /// Minimal success dices count to roll success.
    /// </summary>
    int Count { get; }
}
