namespace ReverieWorld.DiceRoll;

/// <summary>
/// Default implementation of the <see cref="ISuccessParameters"/> interface.
/// </summary>
public class SuccessParameters : ISuccessParameters
{
    /// <summary>
    /// Default value for <see cref="AutoSuccessThreshold" /> parameter.
    /// </summary>
    public const int DefaultAutoSuccessThreshold = 10;

    /// <inheritdoc/>
    public required int MinValue { get; init; }

    /// <inheritdoc/>
    public int Count { get; init; } = 1;

    /// <inheritdoc/>
    public int AutoSuccessThreshold { get; init; } = DefaultAutoSuccessThreshold;
}
