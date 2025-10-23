using UnityEngine;

public class StatsLevelManager : StatDataHelper
{
    private const int ScoreStatsCoefficient = 10;
    private const int ScoreLevelCoefficient = 100;
    private CharacterProgressData _currentCharacter;

    public void Initialize()
    {
        _currentCharacter = Progress.Instance.PlayerInfo.CurrentCharacter;
    }

    public void UpdateStatLevel(Stats stat)
    {
        Debug.Log($"[StatsLevelManager] Leveling up {stat}");

        if (_currentCharacter == null) return;

        switch (stat)
        {
            case Stats.Balks:
                _currentCharacter.LvlBalk++;
                Progress.Instance.PlayerInfo.Score += ScoreStatsCoefficient * _currentCharacter.LvlBalk;
                _currentCharacter.BalksUpdatePoint--;
                break;
            case Stats.Bench:
                _currentCharacter.LvlBench++;
                Progress.Instance.PlayerInfo.Score += ScoreStatsCoefficient * _currentCharacter.LvlBench;
                _currentCharacter.BenchUpdatePoint--;
                break;
            case Stats.HorizontalBar:
                _currentCharacter.LvlHorizontalBars++;
                Progress.Instance.PlayerInfo.Score += ScoreStatsCoefficient * _currentCharacter.LvlHorizontalBars;
                _currentCharacter.HorizontalBarsUpdatePoint--;
                break;
            case Stats.Foots:
                _currentCharacter.LvlFoots++;
                Progress.Instance.PlayerInfo.Score += ScoreStatsCoefficient * _currentCharacter.LvlFoots;
                _currentCharacter.FootsUpdatePoint--;
                break;
        }

        Debug.Log($"[StatsLevelManager] {stat} leveled up. New levels - " +
                 $"Balks: {_currentCharacter.LvlBalk}, " +
                 $"Bench: {_currentCharacter.LvlBench}, " +
                 $"HorizontalBar: {_currentCharacter.LvlHorizontalBars}, " +
                 $"Foots: {_currentCharacter.LvlFoots}");
        
        Progress.Instance.Save();
    }

    public void AddUpdatePoint(Stats stat)
    {
        Debug.Log($"[StatsLevelManager] Adding update point for {stat}");        

        if (_currentCharacter == null) return;

        switch (stat)
        {
            case Stats.Balks: _currentCharacter.BalksUpdatePoint++; break;
            case Stats.Bench: _currentCharacter.BenchUpdatePoint++; break;
            case Stats.HorizontalBar: _currentCharacter.HorizontalBarsUpdatePoint++; break;
            case Stats.Foots: _currentCharacter.FootsUpdatePoint++; break;
        }

        Debug.Log($"[StatsLevelManager] Update points - " +
                 $"Balks: {_currentCharacter.BalksUpdatePoint}, " +
                 $"Bench: {_currentCharacter.BenchUpdatePoint}, " +
                 $"HorizontalBar: {_currentCharacter.HorizontalBarsUpdatePoint}, " +
                 $"Foots: {_currentCharacter.FootsUpdatePoint}");

        Progress.Instance.Save();
    }

    public void UpdateLevel()
    {
        Progress.Instance.PlayerInfo.CurrentCharacter.Level++;
        Progress.Instance.PlayerInfo.Score += ScoreLevelCoefficient * Progress.Instance.PlayerInfo.CurrentCharacter.Level;
        Progress.Instance.Save();
    }
}