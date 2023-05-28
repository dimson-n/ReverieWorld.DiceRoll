using ReverieWorld.DiceRoll.Modifiers;

namespace ReverieWorld.DiceRoll.Tests;

internal sealed class NullModifier : IRollModifier
{
    public void AtRollBegin(IRollState rollState) { }

    public void AtRollEnd(IRollState rollState) { }
}
