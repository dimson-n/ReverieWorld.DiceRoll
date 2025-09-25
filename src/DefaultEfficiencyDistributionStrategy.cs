namespace ReverieWorld.DiceRoll;

/// <summary>
/// Default implementation of efficiency distribution strategy.
/// </summary>
public class DefaultEfficiencyDistributionStrategy : IEfficiencyDistributionStrategy
{
    /// <inheritdoc/>
    public void Distribute(IEfficiencyDistributor distributor, int availableEfficiency, int availableBursts)
    {
        var maxValue = distributor.Parameters.FacesCount;
        var minSuccessValue = distributor.SuccessParameters.MinValue;
        bool successIsMax = maxValue == minSuccessValue;

        var ordered = distributor.Values.OrderByDescending(d => d.Value);
        foreach (var dice in ordered.SkipWhile(dice => dice.Value >= minSuccessValue))
        {
            var needToSuccess = minSuccessValue - dice.Value;
            if (needToSuccess <= availableEfficiency)
            {
                availableEfficiency -= distributor.AddEfficiency(dice, needToSuccess);
                availableBursts -= Convert.ToInt32(successIsMax);
            }
            else
            {
                break;
            }
        }

        if (availableEfficiency == 0 || successIsMax || availableBursts <= 0)
        {
            return;
        }

        foreach (var dice in ordered.SkipWhile(dice => dice.Value == maxValue).Take(availableBursts))
        {
            var needToBurst = maxValue - dice.Value;
            if (needToBurst <= availableEfficiency)
            {
                availableEfficiency -= distributor.AddEfficiency(dice, needToBurst);
            }
            else
            {
                break;
            }
        }
    }
}
