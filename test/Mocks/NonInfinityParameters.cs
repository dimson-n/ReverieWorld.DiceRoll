namespace RP.ReverieWorld.DiceRoll.Tests;

internal sealed class NonInfinityParameters : Parameters
{
    public override bool HasInfinityRerolls => false;
    public override bool HasInfinityBursts => false;

    public NonInfinityParameters(int dicesCount = 1, int additionalDicesCount = 0,
                                 int rerollsCount = 0, int burstsCount = 0, int bonus = 0) :
        base(dicesCount, additionalDicesCount, rerollsCount, burstsCount, bonus)
    {
    }
}
