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
}
