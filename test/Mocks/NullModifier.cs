using ReverieWorld.DiceRoll.Modifiers;

namespace ReverieWorld.DiceRoll.Tests;

internal class NullModifier : IRollModifier
{
    public void AtRollBegin(IRollState rollState) { }

    public void AtRollEnd(IRollState rollState) { }
}
