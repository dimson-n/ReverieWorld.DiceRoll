namespace RP.ReverieWorld.DiceRoll.Tests;

public class ParametersTest
{
    [Fact]
    public void TestValidation()
    {
        Assert.Throws<ArgumentNullException>("parameters", () => Parameters.Validate(null!));

        Assert.Throws<ArgumentOutOfRangeException>("FacesCount",           () => Parameters.Validate(new Parameters(facesCount: 1)));
        Assert.Throws<ArgumentOutOfRangeException>("DicesCount",           () => Parameters.Validate(new Parameters(dicesCount: 0)));
        Assert.Throws<ArgumentOutOfRangeException>("AdditionalDicesCount", () => Parameters.Validate(new Parameters(additionalDicesCount: -1)));

        Assert.Throws<ArgumentException>("RerollsCount", () => Parameters.Validate(new NonInfinityParameters(rerollsCount: Parameters.Infinite)));
        Assert.Throws<ArgumentException>("BurstsCount",  () => Parameters.Validate(new NonInfinityParameters(burstsCount:  Parameters.Infinite)));
    }
}
