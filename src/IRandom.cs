namespace RP.ReverieWorld.DiceRoll;

public interface IRandom : IDisposable
{
    /// <summary>
    /// Same behavior as <see cref="System.Random.Next(int)"/> expected.
    /// </summary>
    /// <param name="maxValue">The exclusive upper bound of the random number to be generated. <paramref name="maxValue"/> must be greater than or equal to 0.</param>
    /// <returns></returns>
    int Next(int maxValue);
}

public interface IRandomProvider
{
    IRandom Lock();
}
