using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace ReverieWorld.DiceRoll;

/// <summary>
/// Represents current state of the dice roll.
/// </summary>
public class Roll : IReadOnlyList<Dice>
{
    private readonly IReadOnlyList<Dice> rolls;

    /// <summary>
    /// Gets parameters of the <see cref="Roll"/>.
    /// </summary>
    /// <value>Parameters of the <see cref="Roll"/>.</value>
    public IParameters Parameters { get; }

    /// <summary>
    /// Gets parameters for success roll.
    /// </summary>
    /// <value>Success parameters of the <see cref="Roll"/> if provided; otherwise <see langword="null"/>.</value>
    [MemberNotNull(nameof(AutoSuccessCount), nameof(SuccessCount))]
    public ISuccessParameters? SuccessParameters { get; }

    /// <summary>
    /// Gets not utilized efficiency bonus value.
    /// </summary>
    /// <value>Not utilized efficiency bonus value.</value>
    public int RemainingEfficiencyBonus { get; }

    /// <summary>
    /// Gets a value indicating whether <see cref="Roll"/> was fully performed.
    /// </summary>
    /// <value><see langword="true"/> if <see cref="Roll"/> was fully performed; otherwise, <see langword="false"/>.</value>
    public virtual bool Completed => false;

    /// <summary>
    /// Gets auto success count.
    /// </summary>
    /// <value>Auto success count if <see cref="SuccessParameters"/> not <see langword="null"/>; otherwise, <see langword="null"/>.</value>
    public int? AutoSuccessCount { get; }

    /// <summary>
    /// Gets count of succeed dices.
    /// </summary>
    /// <value>Count of succeed dices if <see cref="SuccessParameters"/> not <see langword="null"/>; otherwise, <see langword="null"/>.</value>
    public int? SuccessCount { get; }

    internal Roll(RollState state)
    {
        rolls = state.Values;
        Parameters = state.Parameters;
        SuccessParameters = state.SuccessParameters;
        RemainingEfficiencyBonus = state.RemainingBonus;

        if (SuccessParameters is not null)
        {
            AutoSuccessCount = Parameters.AutoSuccesses + Parameters.DicesCount - Math.Min(Parameters.DicesCount, SuccessParameters.AutoSuccessThreshold);
            SuccessCount = rolls.Count(d => d.Value >= SuccessParameters.MinValue) + AutoSuccessCount;
        }
    }

    /// <summary>
    /// Gets the <see cref="Dice"/> at the specified index in the <see cref="Roll"/>.
    /// </summary>
    /// <param name="index">The zero-based index of the <see cref="Dice"/> to get.</param>
    /// <value>The <see cref="Dice"/> at the specified index in the <see cref="Roll"/>.</value>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public Dice this[int index] => rolls[index];

    /// <summary>
    /// Gets the number of <see cref="Dice"/>s in the <see cref="Roll"/>.
    /// </summary>
    /// <value>The number of <see cref="Dice"/>s in the <see cref="Roll"/>.</value>
    public int Count => rolls.Count;

    /// <summary>
    /// Returns an enumerator that iterates through the collection of <see cref="Dice"/>s.
    /// </summary>
    /// <returns>An enumerator for <see cref="IReadOnlyList{Dice}"/>.</returns>
    public IEnumerator<Dice> GetEnumerator() => rolls.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)rolls).GetEnumerator();
}
