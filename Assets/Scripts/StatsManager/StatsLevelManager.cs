using UnityEngine;

public class StatsLevelManager : MonoBehaviour
{
    private static StatsLevelManager instance;

    private CharacterProgressData _currentCharacter;
    private UpdateManager _updateManager;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        WaitForDependencies();
    }

    private void OnDestroy()
    {
        if (StatsManager.Instance != null)
            StatsManager.Instance.OnAddUpdatePoint -= AddUpdatePoint;

        if (_updateManager != null)
            _updateManager.OnUpdatingStatLvl -= UpdateStatLevel;
    }

    private void WaitForDependencies()
    {
        WaitingLoad.Instance.WaitAndExecute(
            () => CharactersDataManager.Instance != null && Progress.Instance?.PlayerInfo?.CurrentCharacter != null,
            () =>
            {
                CharactersDataManager.Instance.OnSelectedCharacter += SetCurrentCharacter;
                SetCurrentCharacter();
            }
        );

        WaitingLoad.Instance.WaitAndExecute(
            () => UpdateManager.Instance != null,
            () =>
            {
                _updateManager = UpdateManager.Instance;
                _updateManager.OnUpdatingStatLvl += UpdateStatLevel;
            }
        );

        WaitingLoad.Instance.WaitAndExecute(
            () => StatsManager.Instance != null,
            () => StatsManager.Instance.OnAddUpdatePoint += AddUpdatePoint
        );
    }

    private void SetCurrentCharacter()
    {
        _currentCharacter = Progress.Instance.PlayerInfo.CurrentCharacter;
        Debug.Log($"[StatsLevelManager] set current character {_currentCharacter.CharacterID}");
    }

    private void UpdateStatLevel(Stats stat)
    {
        Debug.Log($"[StatsLevelManager] Leveling up {stat}");

        if (_currentCharacter == null) return;

        switch (stat)
        {
            case Stats.Balks:
                _currentCharacter.LvlBalk++;
                _currentCharacter.BalksUpdatePoint--;
                break;
            case Stats.Bench: 
                _currentCharacter.LvlBench++;
                _currentCharacter.BenchUpdatePoint--;
                break;
            case Stats.HorizontalBar:
                _currentCharacter.LvlHorizontalBars++;
                _currentCharacter.HorizontalBarsUpdatePoint--;
                break;
            case Stats.Foots:
                _currentCharacter.LvlFoots++;
                _currentCharacter.FootsUpdatePoint--;
                break;
        }

        Debug.Log($"[StatsLevelManager] {stat} leveled up. New levels - " +
                 $"Balks: {_currentCharacter.LvlBalk}, " +
                 $"Bench: {_currentCharacter.LvlBench}, " +
                 $"HorizontalBar: {_currentCharacter.LvlHorizontalBars}, " +
                 $"Foots: {_currentCharacter.LvlFoots}");

        _updateManager.OnStatsLvlUpdated?.Invoke(stat);
        _updateManager.OnCheckUpdateStatsLvl?.Invoke(stat);
        Progress.Instance.Save();
    }

    private void AddUpdatePoint(Stats stat)
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

        _updateManager.OnCheckUpdateStatsLvl?.Invoke(stat);
        Progress.Instance.Save();
    }
}