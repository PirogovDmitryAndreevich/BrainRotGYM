using System;
using UnityEngine;

[System.Serializable]
public class CharacterProgressData
{
    public CharactersEnum CharacterID;

    private int _level = 1;

    public int _balk = 0;
    public int _bench = 0;
    public int _horizontalBars = 0;
    public int _foots = 0;

    public int _lvlBalk = 1;
    public int _lvlBench = 1;
    public int _lvlHorizontalBars = 1;
    public int _lvlFoots = 1;

    public int _balksUpdatePoint = 0;
    public int _benchUpdatePoint = 0;
    public int _horizontalBarsUpdatePoint = 0;
    public int _footsUpdatePoint = 0;

    public Action<Stats> OnAnyStatsChanged;
    public Action<Stats> OnAnyStatsLevelChanged;
    public Action<Stats> OnAnyStatsAddedUpdatePoint;

    public Action OnLevelChanged;

    public int Balk
    {
        get => _balk;
        set
        {
            if (_balk != value)
            {
                _balk = value;
                OnAnyStatsChanged?.Invoke(Stats.Balks);
            }
        }
    }
    public int Bench
    {
        get => _bench;
        set
        {
            if (_bench != value)
            {
                _bench = value;
                OnAnyStatsChanged?.Invoke(Stats.Bench);
            }
        }
    }
    public int HorizontalBars
    {
        get => _horizontalBars;
        set
        {
            if (_horizontalBars != value)
            {
                _horizontalBars = value;
                OnAnyStatsChanged?.Invoke(Stats.HorizontalBar);
            }
        }
    }
    public int Foots
    {
        get => _foots;
        set
        {
            if (_foots != value)
            {
                _foots = value;
                OnAnyStatsChanged?.Invoke(Stats.Foots);
            }
        }
    }

    public int Level
    {
        get => _level;
        set
        {
            if (_level != value)
            {
                _level = value;
                OnLevelChanged?.Invoke();
            }
        }
    }

    public int LvlBalk
    {
        get => _lvlBalk;
        set
        {
            if (_lvlBalk != value)
            {
                _lvlBalk = value;
                OnAnyStatsLevelChanged?.Invoke(Stats.Balks);
            }
        }
    }
    public int LvlBench
    {
        get => _lvlBench;
        set
        {
            if (_lvlBench != value)
            {
                _lvlBench = value;
                OnAnyStatsLevelChanged?.Invoke(Stats.Bench);
            }
        }
    }
    public int LvlHorizontalBars
    {
        get => _lvlHorizontalBars;
        set
        {
            if (_lvlHorizontalBars != value)
            {
                _lvlHorizontalBars = value;
                OnAnyStatsLevelChanged?.Invoke(Stats.HorizontalBar);
            }
        }
    }
    public int LvlFoots
    {
        get => _lvlFoots;
        set
        {
            if (_lvlFoots != value)
            {
                _lvlFoots = value;
                OnAnyStatsLevelChanged?.Invoke(Stats.Foots);
            }
        }
    }

    public int BalksUpdatePoint
    {
        get => _balksUpdatePoint;
        set
        {
            if (_lvlFoots != value)
            {
                _balksUpdatePoint = value;
                OnAnyStatsAddedUpdatePoint?.Invoke(Stats.Balks);
}
        }
    }
    public int BenchUpdatePoint
    {
        get => _benchUpdatePoint;
        set
        {
            if (_benchUpdatePoint != value)
            {
                _benchUpdatePoint = value;
                OnAnyStatsAddedUpdatePoint?.Invoke(Stats.Bench);
            }
        }
    }
    public int HorizontalBarsUpdatePoint
    {
        get => _horizontalBarsUpdatePoint;
        set
        {
            if (_horizontalBarsUpdatePoint != value)
            {
                _horizontalBarsUpdatePoint = value;
                OnAnyStatsAddedUpdatePoint?.Invoke(Stats.HorizontalBar);
            }
        }
    }
    public int FootsUpdatePoint
    {
        get => _footsUpdatePoint;
        set
        {
            if (_footsUpdatePoint != value)
            {
                _footsUpdatePoint = value;
                OnAnyStatsAddedUpdatePoint?.Invoke(Stats.Foots);
            }
        }
    }

    public bool IsLevelRequiresUpdate = false;
    public bool IsStatsRequiresUpdate = false;

    public void UpdateLevel()
    {
        Level = Mathf.Min(LvlBalk, LvlBench, LvlHorizontalBars, LvlFoots);
    }


    public CharacterProgressData() { }

    public CharacterProgressData(CharactersEnum id)
    {
        CharacterID = id;
    }
}