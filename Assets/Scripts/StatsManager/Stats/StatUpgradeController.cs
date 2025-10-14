using UnityEngine;

public class StatUpgradeController : StatDataHelper
{
    private Stats _statType;
    private int _currentLevel;
    private bool _levelUpChecked = false;

    public void Initialize(Stats statType)
    {
        _statType = statType;
        _currentLevel = GetCurrentLevel(statType);
    }

    public bool CheckLevelUp(int currentValue)
    {
        if (IsMaxLevel(_currentLevel) || _levelUpChecked)
            return false;

        int nextLevelThreshold = LEVEL_THRESHOLDS[_currentLevel - 1];

        if (currentValue >= nextLevelThreshold)
        {
            StatsManager.Instance.OnAddUpdatePoint?.Invoke(_statType);
            _levelUpChecked = true;
            _currentLevel++;
            return true;
        }

        return false;
    }

    public void ResetLevelUpCheck() => _levelUpChecked = false;
    public bool IsMaxLevelReached() => IsMaxLevel(_currentLevel);
}