namespace ReverieWorld.DiceRoll.Modifiers;

/// <summary>
/// Represents base interface for roll modifiers.
/// </summary>
public interface IRollModifier
{
    /// <remarks>
    /// Called once at the roll beginning.
    /// </remarks>
    /// <param name="rollState">An object that contains current roll state and allows to modify it.</param>
    void AtRollBegin(IRollState rollState);

    /// <remarks>
    /// Called once at the roll end.
    /// </remarks>
    /// <param name="rollState">An object that contains current roll state and allows to modify it.</param>
    void AtRollEnd(IRollState rollState);
}
