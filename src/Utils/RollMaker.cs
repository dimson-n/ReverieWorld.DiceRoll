namespace ReverieWorld.DiceRoll.Utils;

internal sealed class RollMaker : IDisposable
{
    private readonly int facesCount;
    private readonly IRandom random;

    internal RollMaker(RollState state)
    {
        facesCount = state.parameters.FacesCount;
        random = state.randomProvider.Lock();
    }

    public int Next() => random.Next(facesCount) + 1;

    void IDisposable.Dispose() => random.Dispose();
}
