namespace ReverieWorld.DiceRoll.Tests;

public sealed class SuccessParametersTest
{
    [Fact]
    public void Validation()
    {
        Assert.Throws<ArgumentNullException>("successParameters", ((ISuccessParameters)null!).Validate);

        Assert.Throws<ArgumentOutOfRangeException>("MinValue",             new SuccessParameters { MinValue = 1 }.Validate);
        Assert.Throws<ArgumentOutOfRangeException>("Count",                new SuccessParameters { MinValue = 2, Count = 0 }.Validate);
        Assert.Throws<ArgumentOutOfRangeException>("AutoSuccessThreshold", new SuccessParameters { MinValue = 2, AutoSuccessThreshold = 0 }.Validate);
    }

    [Fact]
    public void Applicability()
    {
        var parameters = new Parameters();

        Assert.Throws<ArgumentOutOfRangeException>("MinValue", () => parameters.ValidateApplicability(new SuccessParameters { MinValue = Parameters.DefaultFacesCount + 1 }));
        Assert.Throws<ArgumentOutOfRangeException>("Count",    () => parameters.ValidateApplicability(new SuccessParameters { MinValue = 2, Count = 2 }));
    }
}
