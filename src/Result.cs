namespace ReverieWorld.DiceRoll;

/// <summary>
/// Represents completed result of the dice <see cref="Roll"/>.
/// </summary>
public sealed class Result : Roll
{
    /// <value>Always <see langword="true"/>.</value>
    /// <inheritdoc/>
    public override bool Completed => true;

    internal Result(RollState state) :
        base(state)
    {
    }

    /// <summary>
    /// Convenience operator for converting to an <see cref="int"/>.
    /// </summary>
    /// <param name="result">The roll result to convert.</param>
    /// <returns>Essentially <see cref="Roll.Total"/>.</returns>
    public static explicit operator int(Result result)
        => result.Total;
}
