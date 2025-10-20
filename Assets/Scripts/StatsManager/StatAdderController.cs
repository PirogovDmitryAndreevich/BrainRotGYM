using System;
using System.Collections.Generic;
using UnityEngine;

public class StatAdderController : StatDataHelper
{
    private CharacterProgressData _currentCharacter;

    public void Initialize()
    {
        _currentCharacter = Progress.Instance.PlayerInfo.CurrentCharacter;
        Debug.Log($"[StatsAdder] select character {_currentCharacter.CharacterID}");
    }

    public void AddingStat(Stats stat, int value)
    {        
        if (_currentCharacter == null) return;

        switch (stat)
        {
            case Stats.Balks:
                _currentCharacter.Balk += value;
                break;
            case Stats.Bench:
                _currentCharacter.Bench += value;
                break;
            case Stats.HorizontalBar:
                _currentCharacter.HorizontalBars += value;
                break;
            case Stats.Foots:
                _currentCharacter.Foots += value;
                break;
            default:
                Debug.LogWarning($"Unknown stat type: {stat}");
                break;
        }

        Progress.Instance?.Save();
    }
}