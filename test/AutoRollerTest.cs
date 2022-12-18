namespace RP.ReverieWorld.DiceRoll.Tests;

public sealed class AutoRollerTest
{
    [Theory]
    [InlineData(1)]
    [InlineData(123)]
    public void MinBound(int count)
    {
        AutoRoller roller = new(new NonRandomZeroProvider(), new Parameters(dicesCount: count));

        var result = roller.Roll();

        Assert.Equal(count, result.Total);
    }

    [Theory]
    [InlineData(3, 1, 3)]
    [InlineData(9, 8, 72)]
    public void MaxBound(int faces, int dices, int expected)
    {
        AutoRoller roller = new(new NonRandomMaxProvider(), new GenericParameters(facesCount: faces, dicesCount: dices));

        var result = roller.Roll();

        Assert.Equal(expected, result.Total);
    }
}
