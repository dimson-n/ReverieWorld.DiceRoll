using System.Collections.Immutable;

namespace RP.ReverieWorld.DiceRoll
{
    /// <summary>
    /// Implements default strategy of <see cref="IDiceRemoveStrategy"/> interface for <see cref="AutoRoller"/>.
    /// </summary>
    public sealed class DefaultDiceRemoveStrategy : IDiceRemoveStrategy
    {
        /// <inheritdoc/>
        public IReadOnlySet<int> Select(IReadOnlyList<Dice> dices, int count, IParameters parameters)
        {
            // TODO: can try boundary+=1 if there is much more rerolls then ones as if parameters.HasInfinityRerolls.
            int nonRerollBoundary = parameters.FacesCount / 2 + (parameters.HasInfinityRerolls ? 1 : 0);

            var sortedRolls = dices.Select((dice, idx) => (dice.Value, idx))
                                   .OrderBy(d => d.Value);
            int mayBeRemoved = Math.Max(count,
                                        sortedRolls.TakeWhile(d => d.Value < nonRerollBoundary)
                                                   .Count());
            int rerollableCount = sortedRolls.TakeWhile(d => d.Value == 1)
                                             .Count();
            int skipToReroll = Math.Min(mayBeRemoved - count,
                                        Math.Min(rerollableCount,
                                                 parameters.HasInfinityRerolls ? int.MaxValue : parameters.RerollsCount));
            int takeFirstRerollable = rerollableCount - skipToReroll;

            return sortedRolls.Where((_, i) => i < takeFirstRerollable || rerollableCount <= i)
                              .Take(count)
                              .Select(d => d.idx)
                              .ToImmutableHashSet();
        }
    }
}
