namespace RP.ReverieWorld.DiceRoll.Tests;

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

        Assert.False(p.HasInfinityRerolls);
        Assert.False(p.HasInfinityBursts);
    }

    [Fact]
    public void InfinityFields()
    {
        Parameters p = new(rerollsCount: Parameters.Infinite, burstsCount: Parameters.Infinite);

        Assert.True(p.HasInfinityRerolls);
        Assert.True(p.HasInfinityBursts);
    }

    [Fact]
    public void Validation()
    {
        Assert.Throws<ArgumentNullException>("parameters", () => Parameters.Validate(null!));

        Assert.Throws<ArgumentOutOfRangeException>("FacesCount",           () => Parameters.Validate(new Parameters(facesCount: 1)));
        Assert.Throws<ArgumentOutOfRangeException>("DicesCount",           () => Parameters.Validate(new Parameters(dicesCount: 0)));
        Assert.Throws<ArgumentOutOfRangeException>("AdditionalDicesCount", () => Parameters.Validate(new Parameters(additionalDicesCount: -1)));

        Assert.Throws<ArgumentException>("RerollsCount", () => Parameters.Validate(new NonInfinityParameters(rerollsCount: Parameters.Infinite)));
        Assert.Throws<ArgumentException>("BurstsCount",  () => Parameters.Validate(new NonInfinityParameters(burstsCount:  Parameters.Infinite)));
    }
}
