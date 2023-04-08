namespace ReverieWorld.DiceRoll;

/// <summary>
/// Represents completed result of the dice <see cref="Roll"/>.
/// </summary>
public sealed class Result : Roll
{
    /// <summary>
    /// Gets a value indicating whether the <see cref="Roll"/> was fully performed.
    /// </summary>
    /// <returns>Always <see langword="true"/>.</returns>
    public override bool Completed => true;

    internal Result(RollState state) :
        base(state)
    {
    }
}
