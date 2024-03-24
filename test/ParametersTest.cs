namespace ReverieWorld.DiceRoll.Tests;

public sealed class ParametersTest
{
    [Fact]
    public void Defaults()
    {
        Parameters p = new();

        Assert.Equal(6, p.FacesCount);
        Assert.Equal(1, p.DicesCount);
        Assert.Equal(0, p.RerollsCount);
        Assert.Equal(0, p.BurstsCount);
        Assert.Equal(0, p.Bonus);
        Assert.Null(p.Modifiers);

        Assert.False(p.HasInfinityRerolls);
        Assert.False(p.HasInfinityBursts);

        Assert.False(p.HasModifiers());
    }

    [Fact]
    public void InfinityFields()
    {
        Parameters p = new(rerollsCount: ParametersBase.Infinite, burstsCount: ParametersBase.Infinite);

        Assert.True(p.HasInfinityRerolls);
        Assert.True(p.HasInfinityBursts);
    }

    [Fact]
    public void Modifiers()
    {
        Parameters p = new(modifier: new NullModifier());

        Assert.NotNull(p.Modifiers);
        Assert.NotEmpty(p.Modifiers);
    }

    [Fact]
    public void Validation()
    {
        Assert.Throws<ArgumentNullException>("parameters", () => ((ParametersBase)null!).Validate());

        Assert.Throws<ArgumentOutOfRangeException>("FacesCount", () => new ParametersBase(facesCount: 1).Validate());

        Assert.Throws<ArgumentOutOfRangeException>("DicesCount", () => new Parameters(dicesCount: 0).Validate());

        Assert.Throws<ArgumentException>("RerollsCount", () => new NonInfinityParameters(rerollsCount: ParametersBase.Infinite).Validate());
        Assert.Throws<ArgumentException>("BurstsCount",  () => new NonInfinityParameters(burstsCount: ParametersBase.Infinite).Validate());

        Assert.Throws<ArgumentOutOfRangeException>("Bonus", () => new Parameters(bonus: -1).Validate());
    }
}
