using ReverieWorld.DiceRoll.Modifiers;

namespace ReverieWorld.DiceRoll.Tests.Modifiers;

public sealed partial class Weber
{
    public sealed class SorcererTrickTest
    {
        static private readonly ISuccessParameters _successParameters = new SuccessParameters { MinValue = 1 };

        [Fact]
        public void OneWithModification()
        {
            AutoRoller roller = new(new NonRandomZeroProvider(),
                                    new Parameters(modifier: new DiceRoll.Modifiers.Weber.SorcererTrick()));

            var result = roller.Roll(_successParameters);

            Assert.Equal(2, result.Count);
            Assert.DoesNotContain(result, d => !d.Modified);
        }

        [Fact]
        public void OneWithoutModification()
        {
            AutoRoller roller = new(new NonRandomMaxProvider(),
                                    new Parameters(modifier: new DiceRoll.Modifiers.Weber.SorcererTrick()));

            var result = roller.Roll(_successParameters);

            Assert.Single(result);
            Assert.DoesNotContain(result, d => d.Modified);
        }

        [Fact]
        public void OneRandom()
        {
            AutoRoller roller = new(new DefaultRandomProvider(),
                                    new Parameters(modifier: new DiceRoll.Modifiers.Weber.SorcererTrick(),
                                                   dicesCount: 3));

            var result = roller.Roll(_successParameters);

            Assert.InRange(result.Count, 3, 4);
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

            var result = roller.Roll(_successParameters);

            Assert.Equal(7, result.Count);
            Assert.Contains(result, d => d.Modified);
            Assert.Contains(result, d => !d.Modified);
        }

        [Fact]
        public void Sequential()
        {
            AutoRoller roller = new(new NonRandomZeroProvider(),
                                    new Parameters(modifier: new DiceRoll.Modifiers.Weber.SorcererTrick()));

            var firstResult = roller.Roll(_successParameters);

            Assert.Equal(2, firstResult.Count);
            Assert.DoesNotContain(firstResult, d => !d.Modified);

            var secondResult = roller.Roll(_successParameters);

            Assert.Single(secondResult);
            Assert.DoesNotContain(secondResult, d => d.Modified);
        }
    }
}
