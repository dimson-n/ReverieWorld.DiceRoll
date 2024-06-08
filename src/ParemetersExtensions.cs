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

        if (parameters.RerollsCount < 0 && !parameters.HasInfinityRerolls)
        {
            throw new ArgumentException("Negative rerolls count available with infinity rerolls only", nameof(parameters.RerollsCount));
        }

        if (parameters.BurstsCount < 0 && !parameters.HasInfinityBursts)
        {
            throw new ArgumentException("Negative bursts count available with infinity bursts only", nameof(parameters.BurstsCount));
        }

        if (parameters.Bonus < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(parameters.Bonus), parameters.Bonus, "Bonus can't be negative");
        }
    }

    /// <summary>
    /// Value that indicates that any <see cref="Modifiers.IRollModifier"/> added by <see cref="IParameters"/>.
    /// </summary>
    /// <returns><see langword="true"/> if <paramref name="parameters"/> contains modifiers; otherwise, <see langword="false"/>.</returns>
    public static bool HasModifiers(this IParameters parameters)
        => parameters.Modifiers is not null && parameters.Modifiers.Count != 0;

    /// <summary>
    /// Validates <paramref name="successParameters"/> correctness.
    /// </summary>
    /// <param name="successParameters">A roll success parameters to validate.</param>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="successParameters"/> is <see langword="null"/>.</exception>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static void Validate(this ISuccessParameters successParameters)
    {
        ArgumentNullException.ThrowIfNull(successParameters);

        if (successParameters.MinValue < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(successParameters.MinValue), successParameters.MinValue, "Dice success value can't be lesser than 1");
        }

        if (successParameters.Count < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(successParameters.Count), successParameters.Count, "Dice success count can't be lesser than 1");
        }
    }

    /// <summary>
    /// Validates applicability of <paramref name="successParameters"/> to given <paramref name="parameters"/>.
    /// </summary>
    /// <param name="parameters">A roll parameters to check.</param>
    /// <param name="successParameters">A roll success parameters to check.</param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static void ValidateApplicability(this IParameters parameters, ISuccessParameters successParameters)
    {
        if (parameters.FacesCount < successParameters.MinValue)
        {
            throw new ArgumentOutOfRangeException(nameof(successParameters.MinValue), successParameters.MinValue, $"Dice roll can't be success for dice with {parameters.FacesCount} faces");
        }

        if (!parameters.HasInfinityBursts && parameters.DicesCount + parameters.BurstsCount < successParameters.Count)
        {
            throw new ArgumentOutOfRangeException(nameof(successParameters.Count), successParameters.Count, $"Insufficient dices count for roll success possibility (max {parameters.DicesCount + parameters.BurstsCount})");
        }
    }
}
