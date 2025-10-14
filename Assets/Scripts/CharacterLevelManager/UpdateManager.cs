using System;
using UnityEngine;

[RequireComponent(typeof(CharacterLevelManager), typeof(StatsLevelManager))]
public class UpdateManager : MonoBehaviour
{
    public static UpdateManager Instance;

    public Action OnCheckUpdateLevel;
    public Action OnRequiresUpdateLevel;
    public Action OnUpdatingLevel;
    public Action OnLevelUpdated;
    public Action<Stats> OnCheckUpdateStatsLvl;
    public Action<Stats> OnRequiresUpdateStatLvl;
    public Action<Stats> OnUpdatingStatLvl;
    public Action<Stats> OnStatsLvlUpdated;

    private CharacterProgressData _currentCharacter;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        OnCheckUpdateStatsLvl += CheckUpdateStats;
        OnCheckUpdateLevel += CheckUpdateLevel;

        WaitForCharacterManager();
    }

    private void OnDestroy()
    {
        if (CharactersDataManager.Instance != null)
            CharactersDataManager.Instance.OnSelectedCharacter -= SetCurrentCharacter;

        OnCheckUpdateStatsLvl -= CheckUpdateStats;
        OnCheckUpdateLevel -= CheckUpdateLevel;
    }

    private void WaitForCharacterManager()
    {
        WaitingLoad.Instance.WaitAndExecute(
            () => CharactersDataManager.Instance != null,
            () => CharactersDataManager.Instance.OnSelectedCharacter += SetCurrentCharacter
        );
    }

    private void SetCurrentCharacter()
    {
        if (Progress.Instance?.PlayerInfo?.CurrentCharacter != null)
        {
            SetCharacterData();
        }
        else
        {
            WaitingLoad.Instance.WaitAndExecute(
                () => Progress.Instance?.PlayerInfo?.CurrentCharacter != null,
                SetCharacterData
            );
        }
    }

    private void SetCharacterData()
    {
        _currentCharacter = Progress.Instance.PlayerInfo.CurrentCharacter;
        CheckUpdateAllStats();
        CheckUpdateLevel();
    }

    private void CheckUpdateAllStats()
    {
        foreach (Stats stat in Enum.GetValues(typeof(Stats)))
            CheckUpdateStats(stat);
    }

    private void CheckUpdateStats(Stats stat)
    {
        int updatePoints = GetUpdatePoints(stat);
        if (updatePoints > 0)
            OnRequiresUpdateStatLvl?.Invoke(stat);
    }

    private void CheckUpdateLevel()
    {
        int minLevel = Mathf.Min(
            _currentCharacter.LvlBalk,
            _currentCharacter.LvlBench,
            _currentCharacter.LvlHorizontalBars,
            _currentCharacter.LvlFoots
        );

        if (_currentCharacter.Level < minLevel)
            OnRequiresUpdateLevel?.Invoke();
    }

    private int GetUpdatePoints(Stats stat)
    {
        return stat switch
        {
            Stats.Balks => _currentCharacter.BalksUpdatePoint,
            Stats.Bench => _currentCharacter.BenchUpdatePoint,
            Stats.HorizontalBar => _currentCharacter.HorizontalBarsUpdatePoint,
            Stats.Foots => _currentCharacter.FootsUpdatePoint,
            _ => 0
        };
    }
}