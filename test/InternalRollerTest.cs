namespace ReverieWorld.DiceRoll.Tests;

/// <summary>
/// Tests for the <see cref="RollState"/> internal class.
/// </summary>
public class InternalRollerTest
{
    [Theory]
    [InlineData(-1)]
    [InlineData(6)]
    public void DiceValueOverflowUnderflow(int diceValue)
    {
        InteractiveRoller roller = new(new PredefinedRandomProvider(diceValue));

        Assert.Throws<ArgumentOutOfRangeException>("value", () => roller.Begin());
    }
}
