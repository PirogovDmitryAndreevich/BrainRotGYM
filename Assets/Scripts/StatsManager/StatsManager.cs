using System;
using System.Collections;
using UnityEngine;

public class StatsManager : MonoBehaviour
{
    public static StatsManager Instance;

    [SerializeField] private StatsType[] _statsUI;

    public Action<Identificate, int> OnAddStat;
    public Action<Stats, int> OnUpdateUIStats;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            OnAddStat += AddingStat;
            StartCoroutine(InitializeWhenReady());
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void OnDestroy()
    {
        OnAddStat -= AddingStat;
    }

    private void InitializeStats()
    {
        foreach (var stat in _statsUI)
        {
            stat.Initialize();

            int currentValue = GetCurrentStatValue(stat.StatType);
            OnUpdateUIStats?.Invoke(stat.StatType, currentValue);
        }
    }

    private IEnumerator InitializeWhenReady()
    {
        // ∆дем Progress.Instance и PlayerInfo
        yield return new WaitUntil(() => Progress.Instance?.PlayerInfo != null);

        InitializeStats();
    }

    private void AddingStat(Identificate stat, int value)
    {
        Stats statType;

        switch (stat)
        {
            case Identificate.Balks:
                Progress.Instance.PlayerInfo.Balk += value;
                statType = Stats.Balks;
                break;
            case Identificate.Bench:
                Progress.Instance.PlayerInfo.Bench += value;
                statType = Stats.Bench;
                break;
            case Identificate.HorizontalBar:
                Progress.Instance.PlayerInfo.HorizontalBars += value;
                statType = Stats.HorizontalBar;
                break;
            case Identificate.Foots:
                Progress.Instance.PlayerInfo.Foots += value;
                statType = Stats.Foots;
                break;
            default:
                Debug.LogWarning($"Unknown stat type: {stat}");
                return;
        }

        Progress.Instance.Save();
        OnUpdateUIStats?.Invoke(statType, GetCurrentStatValue(statType));
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
