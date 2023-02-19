namespace RP.ReverieWorld.DiceRoll.Modifiers;

/// <summary>
/// Provides an interface for dice roll manipulation for roll modifiers.
/// </summary>
public interface IRollState : IReadOnlyList<Dice>
{
    /// <summary>
    /// Gets a read-only list of dices of the current roll state.
    /// </summary>
    /// <returns>A read-only list of dices of the current roll state.</returns>
    IReadOnlyList<Dice> Values { get; }

    /// <summary>
    /// Gets the parameters of the current roll.
    /// </summary>
    /// <returns>The parameters of the current roll.</returns>
    IParameters Parameters { get; }

    /// <summary>
    /// Appends a <paramref name="newValue"/> with designated <paramref name="index"/> to <see cref="Dice"/> list of rolls.
    /// </summary>
    /// <param name="index">The zero-based index of the <see cref="Dice"/> to change.</param>
    /// <param name="newValue">The new value to apply to the <see cref="Dice"/>.</param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    void ChangeValue(int index, int newValue);

    /// <summary>
    /// Add new random <see cref="Dice"/> to current roll.
    /// </summary>
    /// <param name="asBurst">Treat new <see cref="Dice"/> as burst.</param>
    void AddDice(bool asBurst = true);

    /// <summary>
    /// Add new <see cref="Dice"/> to current roll with designated value.
    /// </summary>
    /// <param name="value">Value for new <see cref="Dice"/>.</param>
    /// <param name="asBurst">Treat new <see cref="Dice"/> as burst.</param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    void AddDice(int value, bool asBurst = true);
}
