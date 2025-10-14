using System;
using System.Collections.Generic;
using UnityEngine;

public class StatsManager : MonoBehaviour
{
    public static StatsManager Instance;

    [SerializeField] private StatsType[] _statsUI;

    public Action<Identificate, int> OnAddStat;
    public Action<Stats, int> OnUpdateUIStats;
    public Action<Stats> OnAddUpdatePoint;

    private CharacterProgressData _currentCharacter;
    private Dictionary<Identificate, Stats> _statMapping;
    private Dictionary<Stats, Func<int>> _valueGetters;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        InitializeStatMappings();
        OnAddStat += AddingStat;

        SubscribeToShowManager();
    }

    private void OnDestroy()
    {
        UnsubscribeFromEvents();
    }

    private void InitializeStats()
    {
        CacheCurrentCharacter(() =>
        {
            foreach (var stat in _statsUI)
            {
                stat.Initialize();
            }
            UpdateAllStatsUI();
        });
    }

    private void InitializeStatMappings()
    {
        // Создаем маппинг для быстрого доступа
        _statMapping = new Dictionary<Identificate, Stats>
        {
            { Identificate.Balks, Stats.Balks },
            { Identificate.Bench, Stats.Bench },
            { Identificate.HorizontalBar, Stats.HorizontalBar },
            { Identificate.Foots, Stats.Foots }
        };

        // Создаем геттеры для значений
        _valueGetters = new Dictionary<Stats, Func<int>>
        {
            { Stats.Balks, () => _currentCharacter?.Balk ?? 0 },
            { Stats.Bench, () => _currentCharacter?.Bench ?? 0 },
            { Stats.HorizontalBar, () => _currentCharacter ?.HorizontalBars ?? 0 },
            { Stats.Foots, () => _currentCharacter ?.Foots ?? 0 }
        };
    }

    private void CacheCurrentCharacter(Action onComplete = null)
    {
        if (Progress.Instance?.PlayerInfo?.CurrentCharacter != null)
        {
            _currentCharacter = Progress.Instance.PlayerInfo.CurrentCharacter;
            onComplete?.Invoke();
        }
        else
        {
            WaitingLoad.Instance.WaitAndExecute(
                () => Progress.Instance?.PlayerInfo?.CurrentCharacter != null,
                () =>
                {
                    _currentCharacter = Progress.Instance.PlayerInfo.CurrentCharacter;
                    onComplete?.Invoke();
                }
            );
        }
    }

    private void SubscribeToShowManager()
    {
        if (ShowManager.Instance != null)
        {
            ShowManager.Instance.OnCharacterInitialize += InitializeStats;
        }
        else
        {
            WaitingLoad.Instance.WaitAndExecute(
                () => ShowManager.Instance != null,
                () => ShowManager.Instance.OnCharacterInitialize += InitializeStats
            );
        }
    }

    private void UnsubscribeFromEvents()
    {
        if (ShowManager.Instance != null)
            ShowManager.Instance.OnCharacterInitialize -= InitializeStats;

        OnAddStat -= AddingStat;
    }

    private void AddingStat(Identificate stat, int value)
    {
        if (_currentCharacter == null)
        {
            Debug.LogWarning("Current character is null, cannot add stat");
            return;
        }

        // Быстрая проверка через маппинг
        if (!_statMapping.TryGetValue(stat, out var statType))
        {
            Debug.LogWarning($"Unknown stat type: {stat}");
            return;
        }

        // Обновляем значение
        UpdateStatValue(statType, value);

        // Сохраняем и обновляем UI
        Progress.Instance?.Save();
        UpdateStatUI(statType);
    }

    private void UpdateStatValue(Stats statType, int value)
    {
        
        if (_currentCharacter == null) return;

        switch (statType)
        {
            case Stats.Balks:
                _currentCharacter.Balk += value;
                break;
            case Stats.Bench:
                _currentCharacter.Bench += value;
                break;
            case Stats.HorizontalBar:
                _currentCharacter.HorizontalBars += value;
                break;
            case Stats.Foots:
                _currentCharacter.Foots += value;
                break;
        }
    }

    private void UpdateStatUI(Stats statType)
    {
        int currentValue = GetCurrentStatValue(statType);
        OnUpdateUIStats?.Invoke(statType, currentValue);
    }

    private int GetCurrentStatValue(Stats statType)
    {
        // Быстрый доступ через заранее созданные геттеры
        return _valueGetters.TryGetValue(statType, out var getter) ? getter() : 0;
    }

    public void UpdateAllStatsUI()
    {
        if (_statsUI == null) return;

        foreach (var stat in _statsUI)
        {
            UpdateStatUI(stat.StatType);
        }
    }
}
