using System;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(StatAdderController))]
public class StatsManager : MonoBehaviour
{
    public static StatsManager Instance;

    [SerializeField] private StatsType[] _statsUI;

    public Action<Stats> OnUpdateStatsLevel;
    public Action<Stats> OnAddPoint;

    public Action<Stats> OnProgressIsFilled;
    public Action<Identificate, int> OnAddStat;
    public Action<Stats> OnUpdateUIStats;

    private StatAdderController _adderStats; 

    private Dictionary<Identificate, Stats> _statMapping;
    private Dictionary<Stats, StatsType> _statsTypeDict = new Dictionary<Stats, StatsType>();

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        _adderStats = GetComponent<StatAdderController>();

        InitializeStatMappings();
        OnAddStat += AddingStat;
        GameManager.Instance.OnAllSystemsReady += FirstInitialize;
    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.OnAllSystemsReady -= FirstInitialize;

        if (UpdateManager.Instance != null)
        UpdateManager.Instance.OnStatsLvlUpdated -= ResetProgressBar;

        if (CharactersDataManager.Instance != null)
            CharactersDataManager.Instance.OnSelectedCharacter -= InitializeStats;

        OnAddStat -= AddingStat;

    }
    private void FirstInitialize()
    {
        CharactersDataManager.Instance.OnSelectedCharacter += InitializeStats;
        UpdateManager.Instance.OnStatsLvlUpdated += ResetProgressBar;

        if (Progress.Instance.PlayerInfo.CurrentCharacter != null)
            InitializeStats();
    }

    private void InitializeStats()
    {
        _adderStats.Initialize();


        foreach (var stat in _statsUI)
        {
            stat?.Initialize();
            _statsTypeDict[stat.StatType] = stat;
            UpdateStatUI(stat.StatType);
        }
    }
    private void AddingStat(Identificate stat, int value)
    {
        if (_statMapping.TryGetValue(stat, out var statType))
        {
            _adderStats.AddingStat(statType, value);
            UpdateStatUI(statType);
        }
    }

    private void UpdateStatUI(Stats statType) => OnUpdateUIStats?.Invoke(statType);
    public void ProgressIsFilled(Stats stats) => UpdateManager.Instance.TryAddUpgradePoint(stats);
    private void ResetProgressBar(Stats stats) => _statsTypeDict[stats].InitializeProgressBar();

    private void InitializeStatMappings()
    {
        _statMapping = new Dictionary<Identificate, Stats>
        {
            [Identificate.Balks] = Stats.Balks,
            [Identificate.Bench] = Stats.Bench,
            [Identificate.HorizontalBar] = Stats.HorizontalBar,
            [Identificate.Foots] = Stats.Foots
        };
    }

}
