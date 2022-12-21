using System.Collections;

using RP.ReverieWorld.DiceRoll.Utils;

namespace RP.ReverieWorld.DiceRoll;

/// <summary>
/// Internal roller implementation.
/// </summary>
internal sealed class RollState : IReadOnlyList<Dice>
{
    internal readonly IParameters parameters;
    internal readonly List<Dice> rolls;

    internal int offset = 0;

    internal int DicesToRemove => parameters.AdditionalDicesCount - rolls.Where(d => d.Removed).Count();

    internal RollState(IParameters parameters)
    {
        this.parameters = parameters;
        this.rolls = new List<Dice>(parameters.DicesCount + parameters.AdditionalDicesCount + (parameters.HasInfinityBursts ? parameters.DicesCount : parameters.BurstsCount));
    }

    internal void FillInitial(RollMaker rollMaker)
    {
        int initialRollsCount = parameters.DicesCount + parameters.AdditionalDicesCount;

        for (int i = 0; i != initialRollsCount; ++i)
        {
            AddDice(rollMaker.Next());
        }
    }

    /// <exception cref="ArgumentNullException"/>
    /// <exception cref="InvalidOperationException"/>
    /// <exception cref="ArgumentOutOfRangeException"/>
    public void RemoveDices(IReadOnlySet<int> indices)
    {
        ArgumentNullException.ThrowIfNull(indices);

        if (DicesToRemove < indices.Count)
        {
            throw new InvalidOperationException("Too many indices to remove provided");
        }

        foreach (int index in indices)
        {
            if (index < 0 || rolls.Count <= index)
            {
                throw new ArgumentOutOfRangeException(nameof(indices), indices, "One of indices out of range");
            }
        }

        foreach (int index in indices)
        {
            rolls[index].Removed = true;
        }
    }

    internal void CompleteRerrolsAndBursts(RollMaker rollMaker)
    {
        ParamCounter availableRerolls = new(parameters.RerollsCount, parameters.HasInfinityRerolls);
        ParamCounter availableBursts  = new(parameters.BurstsCount,  parameters.HasInfinityBursts);

        bool somethingChanged = false;
        do
        {
            somethingChanged = false;

            if (availableRerolls.Exists)
            {
                var toReroll = rolls.Where(d => !d.Removed && d.Value == 1);
                while (toReroll.Any() && availableRerolls.Exists)
                {
                    foreach (var d in toReroll.Take(availableRerolls.MaxCount))
                    {
                        --availableRerolls;
                        d.Value = rollMaker.Next();
                    }

                    somethingChanged = true;
                    ++offset;
                }
            }

            if (availableBursts.Exists)
            {
                var toBurst = rolls.Where(d => !d.Removed && !d.burstMade && d.Value == parameters.FacesCount);
                List<Dice> newRolls = new(toBurst.Count());

                while (toBurst.Any() && availableBursts.Exists)
                {
                    newRolls.Clear();
                    foreach (var d in toBurst.Take(availableBursts.MaxCount))
                    {
                        newRolls.Add(new Dice(rollMaker.Next(), offset, isBurst: true));
                        d.burstMade = true;
                        --availableBursts;
                    }
                    rolls.AddRange(newRolls);

                    somethingChanged = true;
                }
            }
        } while (somethingChanged);
    }

    internal void AddDice(int value, bool asBurst = false, bool fromModifier = false)
    {
        rolls.Add(new Dice(value, offset, asBurst, fromModifier));
    }

    public IReadOnlyList<Dice> Values => rolls.AsReadOnly();

    public Dice this[int index] => rolls[index];

    public int Count => rolls.Count;

    public IEnumerator<Dice> GetEnumerator() => rolls.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)rolls).GetEnumerator();
}
