public class StatsManagerCondition : IInitializationCondition
{
    public string ConditionName => "StatsManager";

    public bool IsConditionMet()
    {
        return StatsManager.Instance != null;
    }
}
