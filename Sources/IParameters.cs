namespace RP.ReverieWorld.DiceRoll
{
    public interface IParameters
    {
        int FacesCount { get; }

        int DicesCount { get; }

        int AdditionalDicesCount { get; }

        int RerollsCount { get; }

        int BurstsCount { get; }

        int Bonus { get; }

        bool HasInfinityRerolls => RerollsCount < 0;

        bool HasInfinityBursts => BurstsCount < 0;
    }
}
