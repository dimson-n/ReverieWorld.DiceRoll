namespace ReverieWorld.DiceRoll.Utils;

internal readonly struct ParamCounter
{
    private readonly int count;
    private readonly bool infinite;

    internal ParamCounter(int count, bool infinite)
    {
        this.count = count;
        this.infinite = infinite;
    }

    public readonly bool Exists => infinite || count != 0;

    public readonly int MaxCount => infinite ? int.MaxValue : count;

    public static ParamCounter operator --(ParamCounter source)
    {
        if (!source.infinite)
        {
            return new(source.count - 1, source.infinite);
        }

        return source;
    }
}
