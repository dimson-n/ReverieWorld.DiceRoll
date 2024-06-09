using System.Collections;
using System.Runtime.CompilerServices;

using ReverieWorld.DiceRoll.Modifiers;
using ReverieWorld.DiceRoll.Utils;

namespace ReverieWorld.DiceRoll;

/// <summary>
/// Internal roller implementation.
/// </summary>
internal sealed class RollState : IRollState
{
    private enum RollStage
    {
        BeforeStart,
        AtDicesAdded,
        AfterEnd,
    }

    private readonly Dictionary<RollStage, Action<IRollState>> modifiersActions;
    private readonly List<Dice> rolls;
    private int offset = 0;

    private ParamCounter _availableRerolls;
    private ParamCounter _availableBursts;

    internal RollMaker? currentRollMaker;

    public readonly IRandomProvider RandomProvider;
    public IParameters Parameters { get; }
    public readonly ISuccessParameters? SuccessParameters;
    public int RemainingBonus { get; private set; }

    public RollState(IRandomProvider randomProvider, IParameters parameters, ISuccessParameters? successParameters)
    {
        RandomProvider = randomProvider;
        Parameters = parameters;
        SuccessParameters = successParameters;
        rolls = new List<Dice>(parameters.DicesCount + (parameters.HasInfinityBursts ? parameters.DicesCount : parameters.BurstsCount));
        modifiersActions = [];

        _availableRerolls = new(parameters.RerollsCount, parameters.HasInfinityRerolls);
        _availableBursts  = new(parameters.BurstsCount,  parameters.HasInfinityBursts);
        RemainingBonus = Parameters.Bonus;

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

    public void FillInitial()
    {
        using RollMaker rollMaker = new(this);
        FillInitial(rollMaker);
    }

    public void FillInitial(RollMaker rollMaker)
    {
        InvokeActionsFor(RollStage.BeforeStart);

        int initialRollsCount = Parameters.DicesCount;
        for (int i = 0; i != initialRollsCount; ++i)
        {
            AddDice(rollMaker.Next());
        }

        InvokeActionsFor(RollStage.AtDicesAdded);
    }

    public void MakeRerollsAndBursts()
    {
        using RollMaker rollMaker = new(this);
        MakeRerollsAndBursts(rollMaker);
    }

    public void MakeRerollsAndBursts(RollMaker rollMaker)
    {
        bool somethingChanged = false;
        do
        {
            somethingChanged = false;

            if (_availableRerolls.Exists)
            {
                var toReroll = rolls.Where(d => !d.Removed && d.Value == 1);
                while (toReroll.Any() && _availableRerolls.Exists)
                {
                    foreach (var d in toReroll.Take(_availableRerolls.MaxCount))
                    {
                        --_availableRerolls;
                        d.RawValue = rollMaker.Next();
                    }

                    somethingChanged = true;
                    ++offset;
                }
            }

            if (_availableBursts.Exists)
            {
                bool burstPerformed = false;
                var toBurst = rolls.Where(d => !d.Removed && !d.burstMade && d.Value == Parameters.FacesCount);
                List<Dice> newRolls = new(toBurst.Count());

                while (toBurst.Any() && _availableBursts.Exists)
                {
                    newRolls.Clear();
                    foreach (var d in toBurst.Take(_availableBursts.MaxCount))
                    {
                        newRolls.Add(new Dice(rollMaker.Next(), offset: offset, isBurst: true));
                        d.burstMade = true;
                        --_availableBursts;
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
    }

    public bool DistributeBonus()
    {
        if (RemainingBonus == 0)
        {
            return false;
        }

        if (SuccessParameters is null)
        {
            return false;
        }

        var ordered = rolls.Where(d => !d.Removed).OrderByDescending(d => d.Value);

        var minSuccessValue = SuccessParameters.MinValue;
        foreach (var dice in ordered.SkipWhile(dice => dice.Value > minSuccessValue))
        {
            var needToSuccess = minSuccessValue - dice.Value;
            if (needToSuccess <= RemainingBonus)
            {
                dice.Bonus += needToSuccess;
                RemainingBonus -= needToSuccess;
            }
            else
            {
                break;
            }
        }

        if (RemainingBonus == 0)
        {
            return false;
        }

        bool newBurstAvailable = false;

        var maxValue = Parameters.FacesCount;
        foreach (var dice in ordered.SkipWhile(dice => dice.Value == maxValue))
        {
            var needToBurst = maxValue - dice.Value;
            var canAdd = Math.Min(needToBurst, RemainingBonus);

            dice.Bonus += canAdd;
            RemainingBonus -= canAdd;

            if (dice.Value == maxValue)
            {
                newBurstAvailable = true;
            }

            if (RemainingBonus == 0)
            {
                break;
            }
        }

        return newBurstAvailable;
    }

    private void AddDice(int value, bool asBurst = false, bool fromModifier = false)
    {
        ThrowIfDiceValueOutOfRange(value);

        rolls.Add(new Dice(rawValue: value, offset: offset, isBurst: asBurst, fromModifier: fromModifier));
    }

    /// <exception cref="ArgumentOutOfRangeException"/>
    private void ThrowIfDiceValueOutOfRange(int value, [CallerArgumentExpression(nameof(value))] string? paramName = null)
    {
        int facesCount = Parameters.FacesCount;
        if (value < 1 || facesCount < value)
        {
            throw new ArgumentOutOfRangeException(paramName, value, $"value out of range [{1}..{facesCount}]");
        }
    }

    private void RegisterActionFor(RollStage rollStage, Action<IRollState> modifierAction)
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
        dice.RawValue = newValue;
        dice.Modified = true;
    }

    public Dice this[int index] => rolls[index];

    public int Count => rolls.Count;

    public IEnumerator<Dice> GetEnumerator() => rolls.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)rolls).GetEnumerator();
}
