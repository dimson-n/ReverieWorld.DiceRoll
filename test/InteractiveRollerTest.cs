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

        Assert.Throws<InvalidOperationException>(() => roller.RemoveDice(0));
        Assert.Throws<InvalidOperationException>(() => roller.RemoveDices(new HashSet<int>()));

        Assert.Throws<InvalidOperationException>(() => roller.Result());

        roller.Begin();
    }

    [Fact]
    public void DicesRemove()
    {
        InteractiveRoller roller = new(new NonRandomMaxProvider(), new Parameters(additionalDicesCount: 3));

        roller.Begin();

        Assert.Throws<InvalidOperationException>(() => roller.Begin());

        Assert.Throws<ArgumentNullException>("indices", () => roller.RemoveDices(null!));

        Assert.Throws<ArgumentOutOfRangeException>("index",   () => roller.RemoveDice(100500));
        Assert.Throws<ArgumentOutOfRangeException>("indices", () => roller.RemoveDices(new HashSet<int>() { 123 }));

        Assert.Equal(3, roller.DicesToRemove);

        roller.RemoveDice(0);

        Assert.Equal(2, roller.DicesToRemove);

        roller.RemoveDices(new HashSet<int>() { 1, 3 });

        Assert.Equal(0, roller.DicesToRemove);

        Assert.Throws<InvalidOperationException>(() => roller.RemoveDice(2));
        Assert.Throws<InvalidOperationException>(() => roller.RemoveDices(new HashSet<int>() { 4 }));

        roller.Result();
    }

    [Fact]
    public void Result()
    {
        InteractiveRoller roller = new(new NonRandomMaxProvider());

        roller.Begin();
        roller.Result();

        Assert.Throws<InvalidOperationException>(() => roller.Begin());

        Assert.Throws<InvalidOperationException>(() => roller.RemoveDice(0));
        Assert.Throws<InvalidOperationException>(() => roller.RemoveDices(new HashSet<int>()));
    }
}
