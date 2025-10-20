using System;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(StatsLevelManager), typeof(LevelUpdateDemonstration), typeof(StatsUpdateDemonstration))]
public class UpdateManager : StatDataHelper
{
    private const int MaxLevel = 5;
    public int _AllUpdatePoints;

    public static UpdateManager Instance;

    public Action UpdateBodyView;

    public Action OnAchievedMaxLevel;
    public Action<Stats> OnAchievedMaxStatLvl;

    public Action OnLevelUpdated;
    public Action<Stats> OnStatsLvlUpdated;

    public Action OnRequiresUpdateLevel;
    public Action<Stats> OnRequiresUpdateStatLvl;

    private CharacterProgressData _currentCharacter;
    private StatsLevelManager _levelManager;
    private LevelUpdateDemonstration _levelDemonstration;
    private StatsUpdateDemonstration _statsDemonstration;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        _levelManager = GetComponent<StatsLevelManager>();
        _levelDemonstration = GetComponent<LevelUpdateDemonstration>();
        _statsDemonstration = GetComponent<StatsUpdateDemonstration>();

        GameManager.Instance.OnAllSystemsReady += Initialize;
    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.OnAllSystemsReady -= Initialize;

        if (CharactersDataManager.Instance != null)
            CharactersDataManager.Instance.OnSelectedCharacter -= SetCurrentCharacter;

    }
    public void TryUpdateStatsLvl(Stats stat)
    {
        if (CheckUpdatePossibility(stat))
        {
            _levelManager.UpdateStatLevel(stat);
            OnStatsLvlUpdated?.Invoke(stat);
            CheckRequiresUpdateStats(stat);
            CheckRequiresUpdateLevel();
            _statsDemonstration.Demonstration();
        }
    }

    public void TryUpdateLevel()
    {
        if (CheckUpdateLevelPossibility())
        {
            _levelManager.UpdateLevel();
            OnLevelUpdated?.Invoke();
            CheckRequiresUpdateLevel();
            _levelDemonstration.Demonstration();
        }
    }

    private void Initialize()
    {        
        CharactersDataManager.Instance.OnSelectedCharacter += SetCurrentCharacter;
        SetCurrentCharacter();
    }

    private void SetCurrentCharacter()
    {
        if (Progress.Instance?.PlayerInfo?.CurrentCharacter != null)
        {
            SetCharacterData();
            _levelManager.Initialize();
        }
        else
        {
            WaitingLoad.Instance.WaitAndExecute(
                () => Progress.Instance?.PlayerInfo?.CurrentCharacter != null,
                () =>
                {
                    SetCharacterData();
                    _levelManager.Initialize();
                }
            );
        }
    }

    private void SetCharacterData()
    {
        _currentCharacter = Progress.Instance.PlayerInfo.CurrentCharacter;
        CheckRequiresUpdateAllStats();
        if (!CheckUpdateLevelPossibility())
            CheckRequiresUpdateLevel();
    }


    public void TryAddUpgradePoint(Stats stat)
    {
        if (CheckUpdatePossibility(stat))
        {
            _levelManager.AddUpdatePoint(stat);            
            CheckRequiresUpdateStats(stat);
        }
    }    

    private bool CheckUpdatePossibility(Stats stat)
    {
        int currentLvl = GetCurrentStatLevel(stat);
        if (currentLvl >= MaxLevel)
        {
            OnAchievedMaxStatLvl?.Invoke(stat);
            return false;
        }
        return true;
    }

    private bool CheckUpdateLevelPossibility()
    {
        if (_currentCharacter.Level >= MaxLevel)
        {
            OnAchievedMaxLevel?.Invoke();
            return false;
        }
        return true;
    }

    private void CheckRequiresUpdateAllStats()
    {
        foreach (Stats stat in Enum.GetValues(typeof(Stats)).Cast<Stats>())
        {
            CheckRequiresUpdateStats(stat);
        }
    }

    private void CheckRequiresUpdateStats(Stats stat)
    {
        if (!CheckUpdatePossibility(stat))
            return;

        int changedPoints = GetCurrentUpdatePoints(stat);

        if (changedPoints > 0)
            OnRequiresUpdateStatLvl?.Invoke(stat);

        _currentCharacter.IsStatsRequiresUpdate = AnyStatRequiresUpdate();
    }

    private bool AnyStatRequiresUpdate()
    {
        foreach (Stats stat in Enum.GetValues(typeof(Stats)).Cast<Stats>())
        {
            if (CheckUpdatePossibility(stat) && GetCurrentUpdatePoints(stat) > 0)
                return true;
        }
        return false;
    }

    private void CheckRequiresUpdateLevel()
    {
        if (!CheckUpdateLevelPossibility()) return;

        int minStatLevel = Mathf.Min(
            _currentCharacter.LvlBalk,
            _currentCharacter.LvlBench,
            _currentCharacter.LvlHorizontalBars,
            _currentCharacter.LvlFoots
        );

        if (_currentCharacter.Level < minStatLevel)
        {
            _currentCharacter.IsLevelRequiresUpdate = true;
            OnRequiresUpdateLevel?.Invoke();
        }
        else
        {
            _currentCharacter.IsLevelRequiresUpdate = false;
        }
    }
}