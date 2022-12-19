using System.Collections;

namespace RP.ReverieWorld.DiceRoll;

/// <summary>
/// Internal roller implementation.
/// </summary>
internal sealed class RollState : IReadOnlyList<Dice>
{
    internal readonly IParameters parameters;
    internal readonly List<Dice> rolls;

    internal int offset = 0;

    internal RollState(IParameters parameters)
    {
        this.parameters = parameters;
        this.rolls = new List<Dice>(parameters.DicesCount + parameters.AdditionalDicesCount + (parameters.HasInfinityBursts ? parameters.DicesCount : parameters.BurstsCount));
    }

    internal void AddDice(int value, bool asBurst = false, bool fromModifier = false)
    {
        rolls.Add(new Dice(value, offset, asBurst, fromModifier));
    }

    public Dice this[int index] => rolls[index];

    public int Count => rolls.Count;

    public IEnumerator<Dice> GetEnumerator() => rolls.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)rolls).GetEnumerator();
}
