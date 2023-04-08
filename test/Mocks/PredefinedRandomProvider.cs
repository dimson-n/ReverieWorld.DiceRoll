namespace ReverieWorld.DiceRoll.Tests;

internal sealed class PredefinedRandomProvider : IRandomProvider, IRandom
{
    private readonly int[] values;
    private int current = 0;

    public PredefinedRandomProvider(params int[] values) => this.values = values;

    public IRandom Lock() => this;

    int IRandom.Next(int maxValue)
    {
        var index = current++;
        if (current == values.Length)
        {
            current = 0;
        }

        return values[index];
    }

    void IDisposable.Dispose() { }
}
