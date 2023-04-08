namespace ReverieWorld.DiceRoll.Modifiers;

/// <summary>
/// Represents interface for modifier, that applies after every bunch of <see cref="Dice"/>s addition.
/// </summary>
public interface IAtDicesAddedModifier : IRollModifier
{
    /// <summary>
    /// Applies modification to the current <paramref name="rollState"/> after every bunch of <see cref="Dice"/>s addition.
    /// </summary>
    /// <remarks>
    /// May be called multiple times in one roll.
    /// <br/>
    /// Invokes when <see cref="Dice"/>s added by usual rolling procedure.
    /// </remarks>
    /// <param name="rollState">An object that contains current roll state and allows to modify it.</param>
    void AtDicesAdded(IRollState rollState);
}
