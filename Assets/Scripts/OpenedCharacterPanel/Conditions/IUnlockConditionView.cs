using System;
using UnityEngine;

public interface IUnlockConditionView
{
    event Action<IUnlockConditionView> OnConditionCompleted;

    void Initialize(UnlockCondition condition, PlayerInfo player);

    bool IsCompleted { get; }
}