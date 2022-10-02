using System.Collections;

namespace RP.ReverieWorld.DiceRoll
{
    /// <summary>
    /// Represents result of dices roll.
    /// </summary>
    public sealed class Result : IReadOnlyList<Dice>
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

        internal Result(List<DiceData> data, IParameters parameters)
        {
            this.rolls = data.Select(d => new Dice(d)).ToArray();
            this.parameters = parameters;

            Total = rolls.Where(d => !d.WasRemoved).Sum(d => d.Value) + Bonus;
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
