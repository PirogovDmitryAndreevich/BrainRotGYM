using System;
using System.Collections;
using UnityEngine;

public class StatsManager : MonoBehaviour
{
    public static StatsManager Instance;

    [SerializeField] private StatsType[] _statsUI;

    public Action<Identificate, int> AddStat;
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
        UpdateUIStats?.Invoke(statType, GetCurrentStatValue(statType));
    }

    private void InitializeStats()
    {
        // Проверяем, готов ли Progress.Instance
        if (Progress.Instance?.PlayerInfo == null)
        {
            Debug.Log("Progress.Instance not ready, delaying initialization...");
            StartCoroutine(DelayedInitializeStats());
            return;
        }

        foreach (var stat in _statsUI)
        {
            stat.Initialize();

            int currentValue = GetCurrentStatValue(stat.StatType);
            UpdateUIStats?.Invoke(stat.StatType, currentValue);
        }
    }

    private IEnumerator DelayedInitializeStats()
    {
        // Ждем один кадр
        yield return null;

        // Пробуем снова
        InitializeStats();
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
