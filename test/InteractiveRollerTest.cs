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

        Assert.Throws<InvalidOperationException>(() => roller.Result());

        roller.Begin();
    }

    [Fact]
    public void Result()
    {
        InteractiveRoller roller = new(new NonRandomMaxProvider());

        roller.Begin();
        roller.Result();

        Assert.Throws<InvalidOperationException>(() => roller.Begin());
        Assert.Throws<InvalidOperationException>(() => roller.Result());
    }
}
