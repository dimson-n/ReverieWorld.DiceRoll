namespace RP.ReverieWorld.DiceRoll.Modifiers;

/// <summary>
/// Represents base class for roll modifiers that change new dices value.
/// </summary>
public abstract class OnesToMaxDiceFlipper : IAtDicesAddedModifier
{
    /// <summary>
    /// Selects collection of dice indices with value 1.
    /// </summary>
    /// <param name="rollState">Current roll state.</param>
    /// <returns>Collection of dice indices with value 1.</returns>
    protected static IEnumerable<int> GetOnesIndices(IRollState rollState)
    {
        return rollState.Values
                        .Select((dice, idx) => (dice.Value, idx))
                        .Where(d => d.Value == 1)
                        .Select(d => d.idx);
    }

    /// <inheritdoc/>
    public abstract void AtDicesAdded(IRollState rollState);

    /// <inheritdoc/>
    public virtual void AtRollBegin(IRollState rollState) { }

    /// <inheritdoc/>
    public virtual void AtRollEnd(IRollState rollState) { }
}
