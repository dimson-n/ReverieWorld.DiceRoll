namespace ReverieWorld.DiceRoll.Tests;

/// <summary>
/// Tests for the <see cref="RollState"/> internal class.
/// </summary>
public sealed class InternalRollerTest
{
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

        var result = roller.Roll();

        Assert.Equal(6, result.Count);
        Assert.Equal(3, result.Where(d => d.IsBurst).Count());

        Assert.Equal(36, result.Total);
    }

    [Fact]
    public void BurstsWithInability()
    {
        AutoRoller roller = new(new NonRandomZeroProvider(), new Parameters(burstsCount: 3));

        var result = roller.Roll();

        Assert.Single(result);
        Assert.DoesNotContain(result, d => d.IsBurst);

        Assert.Equal(1, result.Total);
    }

    [Fact]
    public void BurstsWithLackOfDices()
    {
        AutoRoller roller = new(new PredefinedRandomProvider(5, 0), new Parameters(burstsCount: 3));

        var result = roller.Roll();

        Assert.Equal(2, result.Count);
        Assert.Single(result.Where(d => d.IsBurst));

        Assert.Equal(7, result.Total);
    }

    [Fact]
    public void Bonus()
    {
        AutoRoller roller = new(new NonRandomMaxProvider(), new Parameters(bonus: 10));

        var result = roller.Roll();

        Assert.Single(result);

        Assert.Equal(16, result.Total);
    }
}
