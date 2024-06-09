namespace ReverieWorld.DiceRoll.Tests;

public sealed class AutoRollerTest
{
    static private readonly ISuccessParameters _successParameters = new SuccessParameters { MinValue = 1 };

    [Theory(Skip = "WIP")]
    [InlineData(1)]
    [InlineData(123)]
    public void MinBound(int count)
    {
        AutoRoller roller = new(new NonRandomZeroProvider(), new Parameters(dicesCount: count));

        var result = roller.Roll(_successParameters);
    }

    [Theory(Skip = "WIP")]
    [InlineData(3, 1, 3)]
    [InlineData(9, 8, 72)]
    public void MaxBound(int faces, int dices, int expected)
    {
        AutoRoller roller = new(new NonRandomMaxProvider(), new ParametersBase(facesCount: faces, dicesCount: dices));

        var result = roller.Roll(_successParameters);
    }
}
