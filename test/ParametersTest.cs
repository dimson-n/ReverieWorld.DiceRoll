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
        Parameters p = new(rerollsCount: GenericParameters.Infinite, burstsCount: GenericParameters.Infinite);

        Assert.True(p.HasInfinityRerolls);
        Assert.True(p.HasInfinityBursts);
    }

    [Fact]
    public void Validation()
    {
        Assert.Throws<ArgumentNullException>("parameters", () => GenericParameters.Validate(null!));

        Assert.Throws<ArgumentOutOfRangeException>("FacesCount", () => GenericParameters.Validate(new GenericParameters(facesCount: 1)));

        Assert.Throws<ArgumentOutOfRangeException>("DicesCount",           () => GenericParameters.Validate(new Parameters(dicesCount: 0)));
        Assert.Throws<ArgumentOutOfRangeException>("AdditionalDicesCount", () => GenericParameters.Validate(new Parameters(additionalDicesCount: -1)));

        Assert.Throws<ArgumentException>("RerollsCount", () => GenericParameters.Validate(new NonInfinityParameters(rerollsCount: GenericParameters.Infinite)));
        Assert.Throws<ArgumentException>("BurstsCount",  () => GenericParameters.Validate(new NonInfinityParameters(burstsCount: GenericParameters.Infinite)));
    }
}
