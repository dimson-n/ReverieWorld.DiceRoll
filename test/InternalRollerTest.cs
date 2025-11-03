using ReverieWorld.DiceRoll.Modifiers;

namespace ReverieWorld.DiceRoll.Tests;

/// <summary>
/// Tests for the <see cref="RollState"/> internal class.
/// </summary>
public sealed class InternalRollerTest : DefaultSuccessParametersUser
{
    [Theory]
    [InlineData(-1)]
    [InlineData(6)]
    public void DiceValueOverflowUnderflow(int diceValue)
    {
        RollState rollState = new(randomProvider: new PredefinedRandomProvider(diceValue),
                                  efficiencyDistributionStrategy: new DefaultEfficiencyDistributionStrategy(),
                                  parameters: Parameters.Default,
                                  null);

        Assert.Throws<ArgumentOutOfRangeException>("value", rollState.FillInitial);
    }

    [Fact]
    public void Bursts()
    {
        AutoRoller roller = new(new NonRandomMaxProvider());

        var result = roller.Roll(new Parameters(dicesCount: 3, burstsCount: 3), _successParameters);

        Assert.Equal(6, result.Count);
        Assert.Equal(3, result.Count(d => d.IsBurst));
    }

    [Fact]
    public void BurstsWithInability()
    {
        AutoRoller roller = new(new NonRandomZeroProvider());

        var result = roller.Roll(new Parameters(burstsCount: 3), _successParameters);

        Assert.Single(result);
        Assert.DoesNotContain(result, d => d.IsBurst);
    }

    [Fact]
    public void BurstsWithLackOfDices()
    {
        AutoRoller roller = new(new PredefinedRandomProvider(5, 0));

        var result = roller.Roll(new Parameters(burstsCount: 3), _successParameters);

        Assert.Equal(2, result.Count);
        Assert.Single(result, d => d.IsBurst);
    }

    [Fact]
    public void NoApplyEfficiencyToBurst()
    {
        AutoRoller roller = new(new PredefinedRandomProvider(4, 4, 0, 3, 2, 2));

        var result = roller.Roll(new Parameters(dicesCount: 4, rerollsCount: Parameters.Infinite, burstsCount: 2, efficiency: 2),
                                 new SuccessParameters { MinValue = 4, Count = 3 });

        var dicesWithBonus = result.Count(dice => dice.EfficiencyBonus != 0);
        Assert.Equal(2, dicesWithBonus);
    }

    [Fact]
    public void ApplyBurstAfterEfficiencyToMaximum()
    {
        AutoRoller roller = new(new PredefinedRandomProvider(2, 1, 0, 3, 1));

        var result = roller.Roll(new Parameters(dicesCount: 4, rerollsCount: Parameters.Infinite, burstsCount: 1, efficiency: 2),
                                 new SuccessParameters { MinValue = 6 });

        var burstsCount = result.Count(dice => dice.IsBurst);
        Assert.Equal(1, burstsCount);
    }

    [Fact]
    public void AutoSuccess()
    {
        AutoRoller roller = new(new NonRandomZeroProvider());

        var result = roller.Roll(new Parameters(dicesCount: 11),
                                 new SuccessParameters { MinValue = 4 });

        Assert.Equal(1, result.SuccessCount);
        Assert.Equal(1, result.AutoSuccessCount);
    }

    [Fact]
    public void EfficiencyOverflow()
    {
        var rollState = new RollState(randomProvider: new NonRandomZeroProvider(),
                                      efficiencyDistributionStrategy: new DefaultEfficiencyDistributionStrategy(),
                                      parameters: new Parameters(efficiency: 1),
                                      null);

        rollState.FillInitial();
        rollState.AddEfficiency(0, 1);

        ((IRollState)rollState).ChangeValue(0, 6);

        Assert.Equal(0, rollState.RemainingEfficiency);
        Assert.Equal(6, rollState[0].Value);
    }
}
