namespace RP.ReverieWorld.DiceRoll.Modifiers;

/// <summary>
/// Contains roll modifiers for Weber fate.
/// </summary>
public static partial class Weber
{
    /// <summary>
    /// Roll modifier for 'sorcerer trick' feature.
    /// </summary>
    public class SorcererTrick : OnesDiceFlipper
    {
        private readonly int maxCount;

        /// <summary>
        /// Available count of <see cref="Dice"/>s to change in current roll.
        /// </summary>
        protected int currentCount;

        /// <summary>
        /// Maximum count of dices to change in one roll.
        /// </summary>
        public int MaxCount
        {
            get => maxCount;
            init
            {
                maxCount = value;
                currentCount = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SorcererTrick"/> modifier with specified count of applications.
        /// </summary>
        /// <param name="maxCount">Maximal count of modifier applications.</param>
        public SorcererTrick(int maxCount = 1)
        {
            MaxCount = maxCount;
        }

        /// <inheritdoc/>
        public override void AtDicesAdded(IRollState rollState)
        {
            if (currentCount <= 0)
            {
                return;
            }

            var newValue = rollState.Parameters.FacesCount;
            foreach (var index in GetOnesIndices(rollState).Take(currentCount))
            {
                rollState.ChangeValue(index, newValue);
                rollState.AddDice();
                --currentCount;
            }
        }
    }
}
