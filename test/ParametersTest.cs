namespace ReverieWorld.DiceRoll.Tests;

public sealed class ParametersTest
{
    [Fact]
    public void Defaults()
    {
        Parameters p = new();

        Assert.Equal(6, p.FacesCount);
        Assert.Equal(1, p.DicesCount);
        Assert.Equal(0, p.AdditionalDicesCount);
        Assert.Equal(0, p.RerollsCount);
        Assert.Equal(0, p.BurstsCount);
        Assert.Equal(0, p.Bonus);
        Assert.Null(p.Modifiers);

        Assert.False(p.HasInfinityRerolls);
        Assert.False(p.HasInfinityBursts);
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
        Assert.Throws<ArgumentNullException>("parameters", () => ParametersBase.Validate(null!));

        Assert.Throws<ArgumentOutOfRangeException>("FacesCount", () => ParametersBase.Validate(new ParametersBase(facesCount: 1)));

        Assert.Throws<ArgumentOutOfRangeException>("DicesCount",           () => ParametersBase.Validate(new Parameters(dicesCount: 0)));
        Assert.Throws<ArgumentOutOfRangeException>("AdditionalDicesCount", () => ParametersBase.Validate(new Parameters(additionalDicesCount: -1)));

        Assert.Throws<ArgumentException>("RerollsCount", () => ParametersBase.Validate(new NonInfinityParameters(rerollsCount: ParametersBase.Infinite)));
        Assert.Throws<ArgumentException>("BurstsCount",  () => ParametersBase.Validate(new NonInfinityParameters(burstsCount: ParametersBase.Infinite)));
    }
}
