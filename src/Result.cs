namespace RP.ReverieWorld.DiceRoll
{
    /// <summary>
    /// Represents completed result of the dice <see cref="Roll"/>.
    /// </summary>
    public sealed class Result : Roll
    {
        /// <summary>
        /// Gets a value indicating whether the roll <see cref="Result"/> was fully performed.
        /// </summary>
        /// <returns>Always <see langword="true"/>.</returns>
        public override bool Completed => true;

        internal Result(IReadOnlyList<Dice> rolls, IParameters parameters) :
            base(rolls, parameters)
        {
        }
    }
}
