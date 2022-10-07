namespace RP.ReverieWorld.DiceRoll
{
    /// <summary>
    /// Represents completed result of dices roll.
    /// </summary>
    public sealed class Result : Roll
    {
        public override bool Completed => true;

        internal Result(IReadOnlyList<Dice> rolls, IParameters parameters) :
            base(rolls, parameters)
        {
        }
    }
}
