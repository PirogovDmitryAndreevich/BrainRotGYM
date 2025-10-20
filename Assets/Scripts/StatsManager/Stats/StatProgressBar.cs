using UnityEngine;
using UnityEngine.UI;
using System;

public class StatProgressBar : StatDataHelper
{
    [SerializeField] private Image _progressBar;
    
    public event Action<Stats> OnProgressBarFilled;
    
    private Stats _statType;
    private int _currentLevel;
    private int _currentValue;
    private float _totalRequiredForNextLevel;
    private bool _isFilled;

    public void Initialize(Stats statType)
    {
        _statType = statType;
        _isFilled = false;

        UpdateProgressSmooth();
    }

    public void UpdateProgressSmooth()
    {
        if (_isFilled) return;

        int currentValue = GetCurrentStatValue(_statType);
        float requiredForNextLevel = GetCurrentTotalValueForNextLevel();

        if (currentValue >= requiredForNextLevel)
        {
            _isFilled = true;
            SetMaxProgress();
            OnProgressBarFilled?.Invoke(_statType);
        }
        else
        {
            UpdateProgressBarView(currentValue, requiredForNextLevel);
        }
    }

    private float GetCurrentTotalValueForNextLevel()
    {
        int currentLevel = GetCurrentStatLevel(_statType);
        int availableUpdatePoints = GetCurrentUpdatePoints(_statType);
        int effectiveLevel = currentLevel + availableUpdatePoints;
        return 500 * (effectiveLevel + 1) * (effectiveLevel + 2);
    }

    private void UpdateProgressBarView(int currentValue, float requiredForNextLevel)
    {
        float fillAmount = CalculateFillAmount(currentValue, requiredForNextLevel);
        _progressBar.fillAmount = fillAmount;
    }

    public void SetMaxProgress()
    {
        _progressBar.fillAmount = 1f;
    }

    private float CalculateFillAmount(int value, float requiredValue)
    {
        if (requiredValue <= 0) return 0f;
        float clampedValue = Mathf.Min(value, requiredValue);
        return clampedValue / requiredValue;
    }
}