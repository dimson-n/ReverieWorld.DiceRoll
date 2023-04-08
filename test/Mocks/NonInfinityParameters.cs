namespace ReverieWorld.DiceRoll.Tests;

internal sealed class NonInfinityParameters : Parameters
{
    public override bool HasInfinityRerolls => false;
    public override bool HasInfinityBursts => false;

    public NonInfinityParameters(int rerollsCount = 0, int burstsCount = 0) :
        base(rerollsCount: rerollsCount, burstsCount: burstsCount)
    {
    }
}
