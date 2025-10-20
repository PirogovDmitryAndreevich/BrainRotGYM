

public class TrainingAreaCondition : IInitializationCondition
{
    public string ConditionName => "TrainingArea";

    public bool IsConditionMet()
    {
        return TrainingAreaController.Instance != null;
    }
}
