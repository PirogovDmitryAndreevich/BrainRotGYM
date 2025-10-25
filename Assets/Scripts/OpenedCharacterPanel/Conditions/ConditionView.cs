using System;
using UnityEngine;

public abstract class ConditionView : MonoBehaviour, IUnlockConditionView
{
    [SerializeField] private GameObject _checkmark;
    [SerializeField] private GameObject _frame;

    public event Action<IUnlockConditionView> OnConditionCompleted;

    public bool IsCompleted { get; private set; }

    public abstract void Initialize(UnlockCondition condition, PlayerInfo player);

    protected void CheckCondition(UnlockCondition condition, PlayerInfo player)
    {
        if (condition == null || player == null)
        {
            Debug.LogWarning("ConditionView.CheckCondition: null references");
            return;
        }

        bool done = condition.IsSatisfied(player);
        ApplyVisualState(done);

        if (done && !IsCompleted)
        {
            IsCompleted = true;
            OnConditionCompleted?.Invoke(this);
        }
    }

    protected void ApplyVisualState(bool done)
    {
        if (_checkmark != null)
            _checkmark.SetActive(done);

        if (_frame != null)
            _frame.SetActive(done);
    }
}
