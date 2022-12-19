namespace RP.ReverieWorld.DiceRoll.Utils;

internal class RollMaker : IDisposable
{
    private readonly int facesCount;
    private readonly IRandom random;

    internal RollMaker(RollState state, IRandomProvider randomProvider)
    {
        facesCount = state.parameters.FacesCount;
        random = randomProvider.Lock();
    }

    public int Next() => random.Next(facesCount) + 1;

    void IDisposable.Dispose() => random.Dispose();
}