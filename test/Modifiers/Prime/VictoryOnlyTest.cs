namespace ReverieWorld.DiceRoll.Tests.Modifiers;

public sealed partial class Prime
{
    public sealed class VictoryOnlyTest
    {
        static private readonly ISuccessParameters _successParameters = new SuccessParameters { MinValue = 1 };

        [Fact]
        public void All()
        {
            var modifier = new DiceRoll.Modifiers.Prime.VictoryOnly();
            AutoRoller roller = new(new NonRandomZeroProvider(),
                                    new Parameters(modifier, dicesCount: 3));

            var result = roller.Roll(_successParameters);

            Assert.Equal(3, modifier.ApplicationsCount);
            Assert.DoesNotContain(result, d => !d.Modified);

            Assert.Equal(3, result.Count);
        }

        [Fact]
        public void Nothing()
        {
            var modifier = new DiceRoll.Modifiers.Prime.VictoryOnly();
            AutoRoller roller = new(new RandomNotOneProvider(),
                                    new Parameters(modifier, dicesCount: 3));

            var result = roller.Roll(_successParameters);

            Assert.Equal(0, modifier.ApplicationsCount);
            Assert.DoesNotContain(result, d => d.Modified);

            Assert.Equal(3, result.Count);
        }
    }
}
