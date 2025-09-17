using ReverieWorld.DiceRoll.Modifiers;

namespace ReverieWorld.DiceRoll.Tests;

internal sealed class NonInfinityParameters : IParameters
{
    public int FacesCount => 2;

    public int DicesCount => 1;

    public int RerollsCount { get; init; }

    public int BurstsCount { get; init; }

    public int Efficiency => 0;

    public int AutoSuccesses => 0;

    public bool HasInfinityRerolls => false;

    public bool HasInfinityBursts => false;

    public IReadOnlyCollection<IRollModifier>? Modifiers => [];
}
