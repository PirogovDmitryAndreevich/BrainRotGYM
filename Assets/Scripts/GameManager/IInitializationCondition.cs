public interface IInitializationCondition
{
    string ConditionName { get; }
    bool IsConditionMet();
}