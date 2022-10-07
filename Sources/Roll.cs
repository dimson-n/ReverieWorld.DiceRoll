using System.Collections;

namespace RP.ReverieWorld.DiceRoll
{
    /// <summary>
    /// Represents current state of dices roll.
    /// </summary>
    public class Roll : IReadOnlyList<Dice>
    {
        private readonly IReadOnlyList<Dice> rolls;

        private readonly IParameters parameters;

        public int Total { get; }
        public int Bonus => parameters.Bonus;

        public int DiceFacesCount => parameters.FacesCount;
        public int BaseDicesCount => parameters.DicesCount;
        public int RemovedDicesCount => parameters.AdditionalDicesCount;
        public int InitialRerollsCount => parameters.RerollsCount;
        public int InitialBurstsCount => parameters.BurstsCount;

        public bool HasInfinityRerolls => parameters.HasInfinityRerolls;
        public bool HasInfinityBursts => parameters.HasInfinityBursts;

        /// <summary>
        /// Gets a value indicating whether <see cref="Roll"/> was fully performed.
        /// </summary>
        /// <returns><see langword="true"/> if <see cref="Roll"/> was fully performed; otherwise, <see langword="false"/>.</returns>
        public virtual bool Completed => false;

        internal Roll(IReadOnlyList<Dice> rolls, IParameters parameters)
        {
            this.rolls = rolls;
            this.parameters = parameters;

            Total = rolls.Where(d => !d.Removed).Sum(d => d.Value) + Bonus;
        }

        public Dice this[int index] => rolls[index];

        public int Count => rolls.Count;

        public IEnumerator<Dice> GetEnumerator() => rolls.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)rolls).GetEnumerator();

        public override string ToString()
        {
            return Total.ToString();
        }
    }
}
