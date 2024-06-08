namespace ReverieWorld.DiceRoll;

/// <summary>
/// Default implementation of the <see cref="ISuccessParameters"/> interface.
/// </summary>
public class SuccessParameters : ISuccessParameters
{
    /// <inheritdoc/>
    public required int MinValue { get; init; }

    /// <inheritdoc/>
    public int Count { get; init; } = 1;
}
