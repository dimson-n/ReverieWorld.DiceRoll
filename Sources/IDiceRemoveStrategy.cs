namespace RP.ReverieWorld.DiceRoll
{
    /// <summary>
    /// Provides an abstraction for an "add then remove" dice mechanic strategy for <see cref="AutoRoller"/>.
    /// </summary>
    public interface IDiceRemoveStrategy
    {
        /// <summary>
        /// </summary>
        /// <param name="dices">List of current dices values.</param>
        /// <param name="count">Count of dices to remove.</param>
        /// <param name="parameters">Parameters of current roll.</param>
        /// <returns>Set of indices that must be removed with exactly <paramref name="count"/> elements.</returns>
        IReadOnlySet<int> Select(IReadOnlyList<Dice> dices, int count, IParameters parameters);
    }
}
