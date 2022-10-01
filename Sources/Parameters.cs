namespace RP.ReverieWorld.DiceRoll
{
    public interface IParameters
    {
        int FacesCount { get; }
        int DicesCount { get; }
        int AdditionalDicesCount { get; }
        int RerollsCount { get; }
        int BurstsCount { get; }
        int Bonus { get; }

        bool HasInfinityRerolls => RerollsCount < 0;
        bool HasInfinityBursts => BurstsCount < 0;
    }

    /// <summary>
    /// Default implementation of <see cref="IParameters"/> interface.
    /// </summary>
    public class Parameters : IParameters
    {
        public const int DefaultDiceFacesCount = 6;
        /// <summary>
        /// Usable for rerolls & bursts.
        /// </summary>
        public const int Infinite = -1;

        public int FacesCount { get; init; }
        public int DicesCount { get; init; }
        public int AdditionalDicesCount { get; init; }
        public int RerollsCount { get; init; }
        public int BurstsCount { get; init; }
        public int Bonus { get; init; }

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
        /// <param name="parameters">Instance of <see cref="IParameters"/> implementation to validation.</param>
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
