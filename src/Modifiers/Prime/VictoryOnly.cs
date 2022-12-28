namespace RP.ReverieWorld.DiceRoll.Modifiers;

/// <summary>
/// Contains roll modifiers for Prime fate.
/// </summary>
public static partial class Prime
{
    /// <summary>
    /// Roll modifier for 'victory only' feature.
    /// </summary>
    public class VictoryOnly : OnesToMaxDiceFlipper
    {
        /// <summary>
        /// Count of changed <see cref="Dice"/>s at current roll.
        /// </summary>
        public int ApplicationsCount { get; protected set; } = 0;

        /// <inheritdoc/>
        public override void AtDicesAdded(IRollState rollState)
        {
            var newValue = rollState.Parameters.FacesCount;
            foreach (var index in GetOnesIndices(rollState))
            {
                rollState.ChangeValue(index, newValue);
                ++ApplicationsCount;
            }
        }

        /// <inheritdoc/>
        public override void AtRollBegin(IRollState rollState) => ApplicationsCount = 0;
    }
}
