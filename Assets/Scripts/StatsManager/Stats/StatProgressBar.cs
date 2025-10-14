using UnityEngine;
using UnityEngine.UI;

public class StatProgressBar : StatDataHelper
{
    [SerializeField] private Image _progressBar;

    private Stats _statType;
    private int _currentLevel;

    public void Initialize(Stats statType)
    {
        _statType = statType;
        _currentLevel = GetCurrentLevel(statType);

        if (IsMaxLevel(_currentLevel))
            SetMaxProgress();
        else
            UpdateProgressImmediate(GetCurrentStatValue(_statType));
    }

    public void UpdateProgressSmooth(int value)
    {
        if (IsMaxLevel(_currentLevel)) return;
        _progressBar.fillAmount = CalculateFillAmount(value);
    }

    public void UpdateProgressImmediate(int value)
    {
        if (IsMaxLevel(_currentLevel)) return;
        _progressBar.fillAmount = CalculateFillAmount(value);
    }

    public void SetMaxProgress() => _progressBar.fillAmount = 1f;

    public void LevelUp()
    {
        _currentLevel++;
        if (IsMaxLevel(_currentLevel))
            SetMaxProgress();
    }

    private float CalculateFillAmount(int value)
    {
        int currentThreshold = LEVEL_THRESHOLDS[_currentLevel - 1];
        int previousThreshold = _currentLevel > 1 ? LEVEL_THRESHOLDS[_currentLevel - 2] : 0;

        float progress = value - previousThreshold;
        float range = currentThreshold - previousThreshold;

        return Mathf.Clamp01(progress / range);
    }
}