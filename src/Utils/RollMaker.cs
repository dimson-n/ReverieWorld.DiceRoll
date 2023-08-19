using System.Diagnostics;

namespace ReverieWorld.DiceRoll.Utils;

internal sealed class RollMaker : IDisposable
{
    private readonly RollState state;

    private readonly int facesCount;
    private readonly IRandom random;

    internal RollMaker(RollState state)
    {
        Debug.Assert(state.currentRollMaker == null, "doubled RollMaker creation");

        this.state = state;
        facesCount = state.parameters.FacesCount;
        random = state.randomProvider.Lock();

        state.currentRollMaker = this;
    }

    public int Next() => random.Next(facesCount) + 1;

    void IDisposable.Dispose()
    {
        random.Dispose();

        Debug.Assert(state.currentRollMaker == this, "trying to remove wrong RollMaker instance");
        state.currentRollMaker = null;
    }
}
