namespace ReverieWorld.DiceRoll.Tests.Modifiers;

public sealed partial class Prime
{
    public sealed class VictoryOnlyTest
    {
        [Fact]
        public void All()
        {
            var modifier = new DiceRoll.Modifiers.Prime.VictoryOnly();
            AutoRoller roller = new(new NonRandomZeroProvider(),
                                    new Parameters(modifier, dicesCount: 3));

            var result = roller.Roll();

            Assert.Equal(3, modifier.ApplicationsCount);
            Assert.DoesNotContain(result, d => !d.Modified);

            Assert.Equal(3,  result.Count);
            Assert.Equal(18, result.Total);
            Assert.True(result.HasModifiers);
        }

        [Fact]
        public void Nothing()
        {
            var modifier = new DiceRoll.Modifiers.Prime.VictoryOnly();
            AutoRoller roller = new(new RandomNotOneProvider(),
                                    new Parameters(modifier, dicesCount: 3));

            var result = roller.Roll();

            Assert.Equal(0, modifier.ApplicationsCount);
            Assert.DoesNotContain(result, d => d.Modified);

            Assert.Equal(3, result.Count);
            Assert.InRange(result.Total, 6, 18);
            Assert.True(result.HasModifiers);
        }
    }
}
