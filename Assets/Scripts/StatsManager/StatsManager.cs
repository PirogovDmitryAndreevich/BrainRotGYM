using System;
using UnityEngine;

public class StatsManager : MonoBehaviour
{
    public static StatsManager Instance;

    [SerializeField] private StatsType[] _statsUI;

    public Action<Stats, int> AddStat;
    public Action<Stats, int> UpdateUIStats;

    private void Awake()
    {
        Debug.Log("StatsPlayer: Instance created");

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            AddStat += AddingStat;
            InitializeStats();
        }
        else
        {
            Destroy(gameObject);
            return;
        }

    }

    private void OnDestroy()
    {
        AddStat -= AddingStat;
    }

    private void AddingStat(Stats stat, int value)
    {
        switch (stat)
        {
            case Stats.Balks:
                Progress.Instance.PlayerInfo.Balk += value;
                break;
            case Stats.Bench:
                Progress.Instance.PlayerInfo.Bench += value;
                break;
            case Stats.HorizontalBar:
                Progress.Instance.PlayerInfo.HorizontalBars += value;
                break;
            case Stats.Foots:
                Progress.Instance.PlayerInfo.Foots += value;
                break;
            default:
                Debug.LogWarning($"Unknown stat type: {stat}");
                return;
        }

        Progress.Instance.Save();
        UpdateUIStats?.Invoke(stat, value);
    }

    private void InitializeStats()
    {
        foreach (var stat in _statsUI)
        {
            stat.Initialize();

            int currentValue = GetCurrentStatValue(stat.StatType);
            UpdateUIStats?.Invoke(stat.StatType, currentValue);
        }
    }

    private int GetCurrentStatValue(Stats statType)
    {
        if (Progress.Instance?.PlayerInfo == null) return 0;

        return statType switch
        {
            Stats.Balks => Progress.Instance.PlayerInfo.Balk,
            Stats.Bench => Progress.Instance.PlayerInfo.Bench,
            Stats.HorizontalBar => Progress.Instance.PlayerInfo.HorizontalBars,
            Stats.Foots => Progress.Instance.PlayerInfo.Foots,
            _ => 0
        };
    }
}
