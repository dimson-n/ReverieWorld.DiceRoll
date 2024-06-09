namespace ReverieWorld.DiceRoll.Tests;

/// <summary>
/// Tests for the <see cref="RollState"/> internal class.
/// </summary>
public sealed class InternalRollerTest
{
    static private readonly ISuccessParameters _successParameters = new SuccessParameters { MinValue = 1 };

    [Theory]
    [InlineData(-1)]
    [InlineData(6)]
    public void DiceValueOverflowUnderflow(int diceValue)
    {
        InteractiveRoller roller = new(new PredefinedRandomProvider(diceValue));

        Assert.Throws<ArgumentOutOfRangeException>("value", () => roller.Begin());
    }

    [Fact]
    public void Bursts()
    {
        AutoRoller roller = new(new NonRandomMaxProvider(), new Parameters(dicesCount: 3, burstsCount: 3));

        var result = roller.Roll(_successParameters);

        Assert.Equal(6, result.Count);
        Assert.Equal(3, result.Where(d => d.IsBurst).Count());
    }

    [Fact]
    public void BurstsWithInability()
    {
        AutoRoller roller = new(new NonRandomZeroProvider(), new Parameters(burstsCount: 3));

        var result = roller.Roll(_successParameters);

        Assert.Single(result);
        Assert.DoesNotContain(result, d => d.IsBurst);
    }

    [Fact]
    public void BurstsWithLackOfDices()
    {
        AutoRoller roller = new(new PredefinedRandomProvider(5, 0), new Parameters(burstsCount: 3));

        var result = roller.Roll(_successParameters);

        Assert.Equal(2, result.Count);
        Assert.Single(result.Where(d => d.IsBurst));
    }

    [Fact]
    public void NoApplyBonusToBurst()
    {
        AutoRoller roller = new(new PredefinedRandomProvider(4, 4, 0, 3, 2, 2));

        var result = roller.Roll(new Parameters(dicesCount: 4, rerollsCount: Parameters.Infinite, burstsCount: 2, bonus: 2),
                                 new SuccessParameters { MinValue = 4, Count = 3 });

        var dicesWithBonus = result.Count(dice => dice.Bonus != 0);
        Assert.Equal(2, dicesWithBonus);
    }
}
