namespace ReverieWorld.DiceRoll;

/// <summary>
/// Provides an abstraction for efficiency distribution strategies.
/// </summary>
public interface IEfficiencyDistributionStrategy
{
    /// <summary>
    /// Distributes available efficiency bonus between dices.
    /// </summary>
    /// <param name="distributor">A way to distribute bonus into the roll.</param>
    /// <param name="availableEfficiency">Efficiency to distribute.</param>
    /// <param name="availableBursts">Bursts left for current roll; <see cref="int.MaxValue"/> if unlimited.</param>
    void Distribute(IEfficiencyDistributor distributor, int availableEfficiency, int availableBursts);
}
