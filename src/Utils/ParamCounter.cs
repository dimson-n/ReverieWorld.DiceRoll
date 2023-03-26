namespace RP.ReverieWorld.DiceRoll.Utils;

internal sealed class ParamCounter
{
    private int count;
    private readonly bool infinity;

    internal ParamCounter(int count, bool infinity)
    {
        this.count = count;
        this.infinity = infinity;
    }

    internal bool Exists => infinity || count != 0;

    internal int MaxCount => infinity ? int.MaxValue : count;

    public static ParamCounter operator --(ParamCounter counter)
    {
        if (!counter.infinity)
        {
            --counter.count;
        }
        return counter;
    }
}