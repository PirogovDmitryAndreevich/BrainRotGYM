using System;
using UnityEngine;

public class StatsLevelManager : StatDataHelper
{
    [SerializeField] private UpdateLevelPopup _updateLevelPopup;
    [SerializeField] private UpdateStatsPopup _updateStatsPopup;

    private const int ScoreStatsCoefficient = 10;
    private const int ScoreLevelCoefficient = 100;
    private CharacterProgressData _currentCharacter;

    private void Awake()
    {
        _updateLevelPopup.OnAcceptClick += AddScore;
        _updateStatsPopup.OnAcceptClick += AddScore;
    }

    private void OnDestroy()
    {
        _updateLevelPopup.OnAcceptClick -= AddScore;
        _updateStatsPopup.OnAcceptClick -= AddScore;
    }

    public void Initialize()
    {
        _currentCharacter = Progress.Instance.PlayerInfo.CurrentCharacter;
    }    

    public void UpdateStatLevel(Stats stat)
    {
        Debug.Log($"[StatsLevelManager] Leveling up {stat}");

        int lvl;
        int newLvl;
        int prizeScore;

        if (_currentCharacter == null) return;

        switch (stat)
        {
            case Stats.Balks:
                lvl =_currentCharacter.LvlBalk;
                _currentCharacter.LvlBalk++;
                newLvl = _currentCharacter.LvlBalk;
                prizeScore = ScoreStatsCoefficient * _currentCharacter.LvlBalk;
                _currentCharacter.BalksUpdatePoint--;
                break;
            case Stats.Bench:
                lvl = _currentCharacter.LvlBench;
                _currentCharacter.LvlBench++;
                newLvl = _currentCharacter.LvlBench;
                prizeScore = ScoreStatsCoefficient * _currentCharacter.LvlBench;
                _currentCharacter.BenchUpdatePoint--;
                break;
            case Stats.HorizontalBar:
                lvl = _currentCharacter.LvlHorizontalBars;
                _currentCharacter.LvlHorizontalBars++;
                newLvl = _currentCharacter.LvlHorizontalBars;
                prizeScore = ScoreStatsCoefficient * _currentCharacter.LvlHorizontalBars;
                _currentCharacter.HorizontalBarsUpdatePoint--;
                break;
            case Stats.Foots:
                lvl = _currentCharacter.LvlFoots;
                _currentCharacter.LvlFoots++;
                newLvl = _currentCharacter.LvlFoots;
                prizeScore = ScoreStatsCoefficient * _currentCharacter.LvlFoots;
                _currentCharacter.FootsUpdatePoint--;
                break;
            default:
                throw new ArgumentException($"[{name}] Unknown stat type: {stat}");
        }

        Progress.Instance.Save();
        _updateStatsPopup.OpenPopup(lvl,newLvl, prizeScore, stat);

        Debug.Log($"[StatsLevelManager] {stat} leveled up. New levels - " +
                 $"Balks: {_currentCharacter.LvlBalk}, " +
                 $"Bench: {_currentCharacter.LvlBench}, " +
                 $"HorizontalBar: {_currentCharacter.LvlHorizontalBars}, " +
                 $"Foots: {_currentCharacter.LvlFoots}");
    }

    public void AddUpdatePoint(Stats stat)
    {
        Debug.Log($"[StatsLevelManager] Adding update point for {stat}");

        if (_currentCharacter == null)
        {
            Debug.LogError("CurrentCharacter is null");
            return;
        }

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
        var level = Progress.Instance.PlayerInfo.CurrentCharacter.Level;
        var newLevel = ++Progress.Instance.PlayerInfo.CurrentCharacter.Level;
        var score = ScoreLevelCoefficient * Progress.Instance.PlayerInfo.CurrentCharacter.Level;
        _updateLevelPopup.OpenPopup(level, newLevel, score);
        Progress.Instance.Save();
    }

    private void AddScore(int score) => Progress.Instance.PlayerInfo.Score += score;
}