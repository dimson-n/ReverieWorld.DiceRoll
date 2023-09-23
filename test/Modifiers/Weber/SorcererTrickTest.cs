using ReverieWorld.DiceRoll.Modifiers;

namespace ReverieWorld.DiceRoll.Tests.Modifiers;

public sealed partial class Weber
{
    public sealed class SorcererTrickTest
    {
        [Fact]
        public void OneWithModification()
        {
            AutoRoller roller = new(new NonRandomZeroProvider(),
                                    new Parameters(modifier: new DiceRoll.Modifiers.Weber.SorcererTrick()));

            var result = roller.Roll();

            Assert.Equal(2, result.Count);
            Assert.DoesNotContain(result, d => !d.Modified);
            Assert.Equal(7, result.Total);
        }

        [Fact]
        public void OneWithoutModification()
        {
            AutoRoller roller = new(new NonRandomMaxProvider(),
                                    new Parameters(modifier: new DiceRoll.Modifiers.Weber.SorcererTrick()));

            var result = roller.Roll();

            Assert.Single(result);
            Assert.DoesNotContain(result, d => d.Modified);
            Assert.Equal(6, result.Total);
        }

        [Fact]
        public void OneRandom()
        {
            AutoRoller roller = new(new DefaultRandomProvider(),
                                    new Parameters(modifier: new DiceRoll.Modifiers.Weber.SorcererTrick(),
                                                   dicesCount: 3));

            var result = roller.Roll();

            Assert.InRange(result.Count, 3, 4);
            Assert.InRange(result.Total, 6, 24);
        }

        [Fact]
        public void TwoWithModification()
        {
            AutoRoller roller = new(new NonRandomZeroProvider(),
                                    new Parameters(dicesCount: 5,
                                                   modifiers: new List<IRollModifier>
                                                   {
                                                       new DiceRoll.Modifiers.Weber.SorcererTrick(),
                                                       new DiceRoll.Modifiers.Weber.SorcererTrick(),
                                                   }));

            var result = roller.Roll();

            Assert.Equal(7, result.Count);
            Assert.Contains(result, d => d.Modified);
            Assert.Contains(result, d => !d.Modified);
            Assert.Equal(17, result.Total);
        }

        [Fact]
        public void Sequential()
        {
            AutoRoller roller = new(new NonRandomZeroProvider(),
                                    new Parameters(modifier: new DiceRoll.Modifiers.Weber.SorcererTrick()));

            var firstResult = roller.Roll();

            Assert.Equal(2, firstResult.Count);
            Assert.DoesNotContain(firstResult, d => !d.Modified);
            Assert.Equal(7, firstResult.Total);

            var secondResult = roller.Roll();

            Assert.Single(secondResult);
            Assert.DoesNotContain(secondResult, d => d.Modified);
            Assert.Equal(1, secondResult.Total);
        }
    }
}
