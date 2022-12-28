namespace RP.ReverieWorld.DiceRoll.Modifiers;

/// <summary>
/// Contains roll modifiers for Weber fate.
/// </summary>
public static partial class Weber
{
    /// <summary>
    /// Roll modifier for 'sorcerer trick' feature.
    /// </summary>
    public class SorcererTrick : OnesToMaxDiceFlipper
    {
        /// <summary>
        /// Maximum count of dices to change.
        /// </summary>
        public int MaxCount { get; init; } = 1;

        /// <summary>
        /// Available count of <see cref="Dice"/>s to change.
        /// </summary>
        protected int currentCount = 0;

        /// <inheritdoc/>
        public override void AtDicesAdded(IRollState rollState)
        {
            if (currentCount > 0)
            {
                var newValue = rollState.Parameters.FacesCount;
                foreach (var index in GetOnesIndices(rollState).Take(currentCount))
                {
                    rollState.ChangeValue(index, newValue);
                    rollState.AddDice();
                    --currentCount;
                }
            }
        }

        /// <summary>
        /// Resets available count of replaces.
        /// </summary>
        /// <inheritdoc/>
        public override void AtRollBegin(IRollState rollState) => currentCount = MaxCount;
    }
}
