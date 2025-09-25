namespace ReverieWorld.DiceRoll;

/// <summary>
/// Provides an interface for efficiency distribution strategies.
/// </summary>
/// <remarks>Consume-only interface.</remarks>
/// <seealso cref="IEfficiencyDistributionStrategy"/>
public interface IEfficiencyDistributor : IReadOnlyList<Dice>
{
    /// <summary>
    /// Gets a read-only list of dices of the current roll state.
    /// </summary>
    /// <value>A read-only list of dices of the current roll state.</value>
    IReadOnlyList<Dice> Values { get; }

    /// <summary>
    /// Gets the parameters of the current roll.
    /// </summary>
    /// <value>The parameters of the current roll.</value>
    IParameters Parameters { get; }

    /// <summary>
    /// Gets parameters for success roll.
    /// </summary>
    /// <value>Success parameters of the current roll.</value>
    ISuccessParameters SuccessParameters { get; }

    /// <summary>
    /// Adds efficiency bonus to dice with designated index.
    /// </summary>
    /// <param name="diceIndex">Index of dice to add efficiency.</param>
    /// <param name="value">Efficiency to add.</param>
    /// <returns>Added efficiency.</returns>
    /// <exception cref="ArgumentOutOfRangeException" />
    int AddEfficiency(int diceIndex, int value);

    /// <summary>
    /// Adds efficiency bonus to designated dice.
    /// </summary>
    /// <param name="dice">The dice to add efficiency.</param>
    /// <param name="value">Efficiency to add.</param>
    /// <returns>Added efficiency.</returns>
    /// <exception cref="ArgumentOutOfRangeException" />
    int AddEfficiency(Dice dice, int value);
}
