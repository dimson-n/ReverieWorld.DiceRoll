using System.Collections;
using System.Runtime.CompilerServices;

using RP.ReverieWorld.DiceRoll.Modifiers;
using RP.ReverieWorld.DiceRoll.Utils;

namespace RP.ReverieWorld.DiceRoll;

/// <summary>
/// Internal roller implementation.
/// </summary>
internal sealed class RollState : IRollState
{
    private delegate void ModifierDelegate(IRollState rollState);

    private enum RollStage
    {
        BeforeStart,
        AtDicesAdded,
        AfterEnd,
    }

    internal readonly IParameters parameters;
    internal readonly IRandomProvider randomProvider;
    internal readonly List<Dice> rolls;

    internal int offset = 0;

    private readonly Dictionary<RollStage, ModifierDelegate> modifiersActions;

    private RollMaker? currentRollMaker;

    internal int DicesToRemove => parameters.AdditionalDicesCount - rolls.Where(d => d.Removed).Count();

    internal RollState(IParameters parameters, IRandomProvider randomProvider)
    {
        this.parameters = parameters;
        this.randomProvider = randomProvider;
        this.rolls = new List<Dice>(parameters.DicesCount + parameters.AdditionalDicesCount + (parameters.HasInfinityBursts ? parameters.DicesCount : parameters.BurstsCount));
        this.modifiersActions = new Dictionary<RollStage, ModifierDelegate>();

        if (parameters.Modifiers is not null)
        {
            foreach (var modifier in parameters.Modifiers)
            {
                RegisterActionFor(RollStage.BeforeStart, modifier.AtRollBegin);
                RegisterActionFor(RollStage.AfterEnd,    modifier.AtRollEnd);

                if (modifier is IAtDicesAddedModifier dicesAddedModifier)
                {
                    RegisterActionFor(RollStage.AtDicesAdded, dicesAddedModifier.AtDicesAdded);
                }
            }
        }
    }

    internal void FillInitial()
    {
        using RollMaker rollMaker = new(this);
        FillInitial(rollMaker);
    }

    internal void FillInitial(RollMaker rollMaker)
    {
        currentRollMaker = rollMaker;

        InvokeActionsFor(RollStage.BeforeStart);

        int initialRollsCount = parameters.DicesCount + parameters.AdditionalDicesCount;

        for (int i = 0; i != initialRollsCount; ++i)
        {
            AddDice(rollMaker.Next());
        }

        InvokeActionsFor(RollStage.AtDicesAdded);

        currentRollMaker = null;
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

    internal void CompleteRerrolsAndBursts()
    {
        using RollMaker rollMaker = new(this);
        CompleteRerrolsAndBursts(rollMaker);
    }

    internal void CompleteRerrolsAndBursts(RollMaker rollMaker)
    {
        ParamCounter availableRerolls = new(parameters.RerollsCount, parameters.HasInfinityRerolls);
        ParamCounter availableBursts  = new(parameters.BurstsCount,  parameters.HasInfinityBursts);

        currentRollMaker = rollMaker;

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
                bool burstPerformed = false;
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

                    burstPerformed = true;
                }

                if (burstPerformed)
                {
                    InvokeActionsFor(RollStage.AtDicesAdded);
                    somethingChanged = true;
                }
            }
        } while (somethingChanged);

        InvokeActionsFor(RollStage.AfterEnd);

        currentRollMaker = null;
    }

    internal void AddDice(int value, bool asBurst = false, bool fromModifier = false)
    {
        ThrowIfDiceValueOutOfRange(value);

        rolls.Add(new Dice(value, offset, asBurst, fromModifier));
    }

    /// <exception cref="ArgumentOutOfRangeException"/>
    private void ThrowIfDiceValueOutOfRange(int value, [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        if (value <= 0 || parameters.FacesCount < value)
        {
            throw new ArgumentOutOfRangeException(paramName, value, $"value out of range [{1}..{parameters.FacesCount}]");
        }
    }

    private void RegisterActionFor(RollStage rollStage, ModifierDelegate modifierAction)
    {
        if (modifiersActions.TryGetValue(rollStage, out var actions))
        {
            modifiersActions[rollStage] = actions + modifierAction;
            return;
        }

        modifiersActions.Add(rollStage, modifierAction);
    }

    private void InvokeActionsFor(RollStage rollStage)
    {
        if (modifiersActions.TryGetValue(rollStage, out var actions))
        {
            actions.Invoke(this);
        }
    }

    public IReadOnlyList<Dice> Values => rolls.AsReadOnly();

    IParameters IRollState.Parameters => parameters;

    void IRollState.AddDice(bool asBurst)
    {
        var rollMaker = currentRollMaker ?? throw new InvalidOperationException($"Logic error: {nameof(currentRollMaker)} is null");
        AddDice(rollMaker.Next(), asBurst, true);
    }

    void IRollState.AddDice(int value, bool asBurst) => AddDice(value, asBurst, true);

    void IRollState.ChangeValue(int index, int newValue)
    {
        ThrowIfDiceValueOutOfRange(newValue);

        var dice = rolls[index];
        dice.Value = newValue;
        dice.Modified = true;
    }

    public Dice this[int index] => rolls[index];

    public int Count => rolls.Count;

    public IEnumerator<Dice> GetEnumerator() => rolls.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)rolls).GetEnumerator();
}
