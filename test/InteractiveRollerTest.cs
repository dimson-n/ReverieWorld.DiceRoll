namespace ReverieWorld.DiceRoll.Tests;

public sealed class InteractiveRollerTest
{
    [Fact]
    public void Ctor()
    {
        Assert.Throws<ArgumentNullException>("randomProvider", () => new InteractiveRoller(null!));
    }

    [Fact]
    public void Init()
    {
        InteractiveRoller roller = new(new NonRandomMaxProvider());

        Assert.False(roller.CanDistributeEfficiency);

        Assert.Empty(roller.Values);

        Assert.Throws<InvalidOperationException>(() => roller.AddEfficiency(0));
        Assert.Throws<InvalidOperationException>(() => roller.AddEfficiency(new Dice(0)));

        Assert.Throws<InvalidOperationException>(roller.Result);

        Assert.False(roller.Current.Completed);
    }

    [Fact]
    public void EfficiencyDistribution()
    {
        InteractiveRoller roller = new(new NonRandomZeroProvider(), new Parameters(dicesCount: 4, efficiency: 10));

        roller.Begin();

        Assert.NotEmpty(roller.Values);

        Assert.Throws<InvalidOperationException>(roller.Begin);

        Assert.Throws<ArgumentOutOfRangeException>(() => roller.AddEfficiency(-1));
        Assert.Throws<ArgumentOutOfRangeException>(() => roller.AddEfficiency(4));
        Assert.Throws<ArgumentOutOfRangeException>("dice", () => roller.AddEfficiency(new Dice(0)));

        Assert.True(roller.CanDistributeEfficiency);

        Assert.Equal(1, roller.AddEfficiency(0));
        Assert.Equal(2, roller.AddEfficiency(1, 2));

        var dices = roller.Values;

        Assert.Equal(3, roller.AddEfficiency(dices[2], 3));
        Assert.Equal(4, roller.AddEfficiency(dices[3], 7));

        Assert.False(roller.CanDistributeEfficiency);

        Assert.Equal(0, roller.AddEfficiency(0, 5));

        Assert.Equal(2, dices[0].Value);
        Assert.Equal(3, dices[1].Value);
        Assert.Equal(4, dices[2].Value);
        Assert.Equal(5, dices[3].Value);

        Assert.False(roller.Current.Completed);
    }

    [Fact]
    public void Result()
    {
        InteractiveRoller roller = new(new NonRandomZeroProvider());

        roller.Begin();
        roller.Result();

        Assert.False(roller.CanDistributeEfficiency);

        Assert.Throws<InvalidOperationException>(roller.Begin);
        Assert.Throws<InvalidOperationException>(roller.Result);

        Assert.Throws<InvalidOperationException>(() => roller.AddEfficiency(0));
        Assert.Throws<InvalidOperationException>(() => roller.AddEfficiency(new Dice(0)));

        Assert.True(roller.Current.Completed);
    }

    [Fact]
    public void CanDistributeEfficiency()
    {
        InteractiveRoller roller = new(new NonRandomZeroProvider(), new Parameters(efficiency: 1));

        roller.Begin();

        Assert.True(roller.CanDistributeEfficiency);
    }

    [Fact]
    public void CanNotDistributeEfficiency()
    {
        InteractiveRoller roller = new(new NonRandomZeroProvider());

        roller.Begin();

        Assert.False(roller.CanDistributeEfficiency);
    }

    [Fact]
    public void CanNotDistributeEfficiencyMax()
    {
        InteractiveRoller roller = new(new NonRandomMaxProvider(), new Parameters(efficiency: 1));

        roller.Begin();

        Assert.False(roller.CanDistributeEfficiency);
    }
}
