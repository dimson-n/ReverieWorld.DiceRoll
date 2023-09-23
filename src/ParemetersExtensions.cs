namespace ReverieWorld.DiceRoll;

/// <summary>
/// Provides extension methods for <see cref="IParameters" />.
/// </summary>
public static class ParametersExtensions
{
    /// <summary>
    /// Validates <paramref name="parameters"/> correctness.
    /// </summary>
    /// <param name="parameters">A roll parameters to validate.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="parameters"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    /// <exception cref="ArgumentException"></exception>
    public static void Validate(this IParameters parameters)
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
