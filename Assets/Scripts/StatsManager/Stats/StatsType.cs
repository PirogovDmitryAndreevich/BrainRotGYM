using System;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(StatProgressBar))]
public class StatsType : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;

    protected Stats _statsType;
    private StatProgressBar _progressBar;
    private bool _isInitialized;
    private bool _isMaxLevel;
    public Stats StatType => _statsType;
    //public event Action<Stats> OnProgressIsFilled;

    private void Awake() => _progressBar = GetComponent<StatProgressBar>();

    private void OnDestroy()
    {
        if (StatsManager.Instance != null)
            StatsManager.Instance.OnUpdateUIStats -= UpdateUI;

            _progressBar.OnProgressBarFilled -= StatsManager.Instance.ProgressIsFilled;
    }

    public virtual void Initialize()
    {
        if (_isInitialized) return;

        StatsManager.Instance.OnUpdateUIStats += UpdateUI;
        _progressBar.OnProgressBarFilled += StatsManager.Instance.ProgressIsFilled;
        UpdateManager.Instance.OnAchievedMaxStatLvl += ShowMaxLevel;

        InitializeProgressBar();
        UpdateUI(_statsType);
        _isInitialized = true;
    }

    public void InitializeProgressBar() => _progressBar.Initialize(_statsType);

    private void UpdateUI(Stats stat)
    {
        if (stat != _statsType || _isMaxLevel) return;

        int currentValue = GetCurrentStatValue();

        _text.text = currentValue.ToString();
        _progressBar.UpdateProgressSmooth();
    }

    private void ShowMaxLevel(Stats stat)
    {
        if (stat != _statsType || _isMaxLevel) return;

        _text.text = "MAX";
        _progressBar.SetMaxProgress();
        _isMaxLevel = true;
    }

    private int GetCurrentStatValue()
    {
        var character = Progress.Instance?.PlayerInfo?.CurrentCharacter;
        return character == null ? 0 : _statsType switch
        {
            Stats.Balks => character.Balk,
            Stats.Bench => character.Bench,
            Stats.HorizontalBar => character.HorizontalBars,
            Stats.Foots => character.Foots,
            _ => throw new ArgumentException($"Unknown stat type: {_statsType}")
        };
    }
}