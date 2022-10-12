namespace RP.ReverieWorld.DiceRoll
{
    /// <summary>
    /// Default implementation of the <see cref="IParameters"/> interface.
    /// </summary>
    public class Parameters : IParameters
    {
        /// <summary>
        /// Default value for <see cref="FacesCount"/> property unless otherwise specified.
        /// </summary>
        public const int DefaultDiceFacesCount = 6;

        /// <summary>
        /// Usable for rerolls and bursts.
        /// </summary>
        public const int Infinite = -1;

        /// <inheritdoc/>
        public virtual int FacesCount { get; init; }

        /// <inheritdoc/>
        public virtual int DicesCount { get; init; }

        /// <inheritdoc/>
        public virtual int AdditionalDicesCount { get; init; }

        /// <inheritdoc/>
        public virtual int RerollsCount { get; init; }

        /// <inheritdoc/>
        public virtual int BurstsCount { get; init; }

        /// <inheritdoc/>
        public virtual int Bonus { get; init; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Parameters"/> class with designated parameters for roll.
        /// </summary>
        /// <param name="facesCount">Dice faces count.</param>
        /// <param name="dicesCount">Count of dices to roll</param>
        /// <param name="additionalDicesCount">Count of dices for "add then remove" dice mechanic.</param>
        /// <param name="rerollsCount">Count of possible rerolls for dices with value 1.</param>
        /// <param name="burstsCount">Count of possible bursts for dices with max possible value.</param>
        /// <param name="bonus">Bonus or penalty value for a roll.</param>
        public Parameters(int facesCount = DefaultDiceFacesCount, int dicesCount = 1, int additionalDicesCount = 0,
                          int rerollsCount = 0, int burstsCount = 0, int bonus = 0)
        {
            FacesCount = facesCount;
            DicesCount = dicesCount;
            AdditionalDicesCount = additionalDicesCount;
            RerollsCount = rerollsCount;
            BurstsCount = burstsCount;
            Bonus = bonus;
        }

        /// <summary>
        /// Validates <paramref name="parameters"/> correctness.
        /// </summary>
        /// <param name="parameters">A roll parameters to validate.</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="parameters"/> is <see langword="null"/>.</exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public static void Validate(IParameters parameters)
        {
            ArgumentNullException.ThrowIfNull(parameters);

            if (parameters.FacesCount < 2)
            {
                throw new ArgumentOutOfRangeException(nameof(parameters.FacesCount), parameters.FacesCount, "Dice faces count can't be lesser than 2");
            }

            if (parameters.DicesCount < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(parameters.DicesCount), parameters.DicesCount, "Can't roll lesser than 1 dice");
            }

            if (parameters.AdditionalDicesCount < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(parameters.AdditionalDicesCount), parameters.AdditionalDicesCount, "Can't use negative count of additional dices");
            }

            if (parameters.RerollsCount < 0 && !parameters.HasInfinityRerolls)
            {
                throw new ArgumentException("Negative rerolls count available with infinity rerolls only", nameof(parameters.RerollsCount));
            }

            if (parameters.BurstsCount < 0 && !parameters.HasInfinityBursts)
            {
                throw new ArgumentException("Negative bursts count available with infinity bursts only", nameof(parameters.BurstsCount));
            }
        }
    }
}
