using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InitializationSystem : MonoBehaviour, IInitializationSystem
{
    public event Action<string> OnConditionMet;
    public event Action OnAllConditionsMet;

    private List<IInitializationCondition> _conditions = new List<IInitializationCondition>();
    private HashSet<string> _metConditions = new HashSet<string>();

    public void RegisterCondition(IInitializationCondition condition)
    {
        if (!_conditions.Contains(condition))
        {
            _conditions.Add(condition);
        }
    }

    public void CheckAllConditions()
    {
        foreach (var condition in _conditions)
        {
            WaitingLoad.Instance.WaitAndExecute(
                condition.IsConditionMet,
                () => OnConditionMetHandler(condition)
            );
        }

        WaitingLoad.Instance.WaitAndExecute(
            () => _metConditions.Count >= _conditions.Count,
            OnAllInitializationComplete
        );
    }

    private void OnConditionMetHandler(IInitializationCondition condition)
    {
        if (_metConditions.Add(condition.ConditionName))
        {
            OnConditionMet?.Invoke(condition.ConditionName);
        }
    }

    private void OnAllInitializationComplete()
    {
        OnAllConditionsMet?.Invoke();
    }
}