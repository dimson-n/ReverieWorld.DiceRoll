using System.Collections.Immutable;

namespace RP.ReverieWorld.DiceRoll
{
    public sealed partial class AutoRoller
    {
        public sealed class DefaultDiceRemovingSelector : IDiceRemovingSelector
        {
            public IReadOnlySet<int> Select(IReadOnlyList<Dice> dices, int count, IParameters parameters)
            {
                // TODO: can try boundary+=1 if there is much more rerolls then ones as if parameters.HasInfinityRerolls.
                int nonRerollBoundary = parameters.FacesCount / 2 + (parameters.HasInfinityRerolls ? 1 : 0);

                var sortedRolls = dices.Select((val, idx) => (val, idx)).OrderBy(d => d.val);
                int mayBeRemoved = Math.Max(count, sortedRolls.TakeWhile(d => d.val.Value < nonRerollBoundary).Count());
                int rerollableCount = sortedRolls.TakeWhile(d => d.val.Value == 1).Count();
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
}
