using UnityEngine;

public class StatDataHelper : MonoBehaviour
{
    protected const int MAX_LEVEL = 5;
    protected readonly int[] LEVEL_THRESHOLDS = new int[] { 1000, 3000, 6000, 10000, 15000 };

    protected int GetCurrentLevel(Stats statType)
    {
        var character = Progress.Instance?.PlayerInfo?.CurrentCharacter;
        return character == null ? 1 : statType switch
        {
            Stats.Balks => character.LvlBalk,
            Stats.Bench => character.LvlBench,
            Stats.HorizontalBar => character.LvlHorizontalBars,
            Stats.Foots => character.LvlFoots,
            _ => 1
        };
    }

    protected int GetCurrentStatValue(Stats statType)
    {
        var character = Progress.Instance?.PlayerInfo?.CurrentCharacter;
        return character == null ? 0 : statType switch
        {
            Stats.Balks => character.Balk,
            Stats.Bench => character.Bench,
            Stats.HorizontalBar => character.HorizontalBars,
            Stats.Foots => character.Foots,
            _ => 0
        };
    }

    protected bool IsMaxLevel(int level) => level >= MAX_LEVEL;
}