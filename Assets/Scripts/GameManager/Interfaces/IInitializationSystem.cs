using System;

public interface IInitializationSystem
{
    event Action<string> OnConditionMet;
    event Action OnAllConditionsMet;
    void RegisterCondition(IInitializationCondition condition);
    void CheckAllConditions();
}