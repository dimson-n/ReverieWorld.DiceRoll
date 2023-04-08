namespace RP.ReverieWorld.DiceRoll.Modifiers;

/// <summary>
/// Represents base interface for roll modifiers.
/// </summary>
public interface IRollModifier
{
    /// <summary>
    /// Called once at the roll beginning.
    /// </summary>
    /// <param name="rollState">An object that contains current roll state and allows to modify it.</param>
    void AtRollBegin(IRollState rollState);

    /// <summary>
    /// Called once at the roll end.
    /// </summary>
    /// <param name="rollState">An object that contains current roll state and allows to modify it.</param>
    void AtRollEnd(IRollState rollState);
}
