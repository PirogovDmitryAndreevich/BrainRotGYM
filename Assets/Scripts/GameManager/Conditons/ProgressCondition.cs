public class ProgressCondition : IInitializationCondition
{
    public string ConditionName => "Progress";


    public bool IsConditionMet()
    {
        return Progress.Instance != null;
    }
}

