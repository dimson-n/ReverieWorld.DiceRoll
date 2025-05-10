namespace ReverieWorld.DiceRoll.Modifiers;

/// <summary>
/// Represents base class for roll modifiers that change new dices value.
/// </summary>
public abstract class OnesDiceFlipper : IAtDicesAddedModifier
{
    /// <summary>
    /// Selects collection of dice indices with value 1.
    /// </summary>
    /// <param name="rollState">Current roll state.</param>
    /// <returns>Collection of dice indices with value 1.</returns>
    protected static IEnumerable<Dice> GetOnesDices(IRollState rollState)
    {
        return rollState.Where(d => d.Value == 1);
    }

    /// <inheritdoc/>
    public abstract void AtDicesAdded(IRollState rollState);

    /// <inheritdoc/>
    public virtual void AtRollBegin(IRollState rollState) { }

    /// <inheritdoc/>
    public virtual void AtRollEnd(IRollState rollState) { }
}
