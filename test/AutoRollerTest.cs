namespace ReverieWorld.DiceRoll.Tests;

public sealed class AutoRollerTest
{
    [Fact]
    public void MaxFail()
    {
        AutoRoller roller = new(new NonRandomZeroProvider());

        var result = roller.Roll(new SuccessParameters { MinValue = Parameters.DefaultFacesCount });

        Assert.Equal(0, result.SuccessCount);
    }

    [Fact]
    public void MaxSuccess()
    {
        AutoRoller roller = new(new NonRandomMaxProvider());

        var result = roller.Roll(new SuccessParameters { MinValue = Parameters.DefaultFacesCount });

        Assert.Equal(1, result.SuccessCount);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(123)]
    public void MinSuccess(int count)
    {
        AutoRoller roller = new(new PredefinedRandomProvider(2));

        var result = roller.Roll(new Parameters(dicesCount: count), new SuccessParameters { MinValue = 2 });

        Assert.Equal(count, result.SuccessCount);
    }
}
