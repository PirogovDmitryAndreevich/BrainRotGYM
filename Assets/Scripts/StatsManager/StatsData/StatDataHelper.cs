using System;
using UnityEngine;

public class StatDataHelper : MonoBehaviour
{
    protected int GetCurrentStatLevel(Stats statType)
    {
        var character = Progress.Instance?.PlayerInfo?.CurrentCharacter;
        return character == null ? 1 : statType switch
        {
            Stats.Balks => character.LvlBalk,
            Stats.Bench => character.LvlBench,
            Stats.HorizontalBar => character.LvlHorizontalBars,
            Stats.Foots => character.LvlFoots,
            _ => throw new ArgumentException($"Unknown stat type: {statType}")
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
            _ => throw new ArgumentException($"Unknown stat type: {statType}")
        };
    }

    protected int GetCurrentUpdatePoints(Stats stat)
    {
        var character = Progress.Instance?.PlayerInfo?.CurrentCharacter;

        return stat switch
        {
            Stats.Balks => character.BalksUpdatePoint,
            Stats.Bench => character.BenchUpdatePoint,
            Stats.HorizontalBar => character.HorizontalBarsUpdatePoint,
            Stats.Foots => character.FootsUpdatePoint,
            _ => throw new ArgumentException($"Unknown stat type: {stat}")
        };
    }
}