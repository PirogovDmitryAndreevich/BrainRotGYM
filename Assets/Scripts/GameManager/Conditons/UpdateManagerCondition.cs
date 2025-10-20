public class UpdateManagerCondition : IInitializationCondition
{
    public string ConditionName => "UpdateManager";

    public bool IsConditionMet()
    {
       return UpdateManager.Instance != null;
    }
}
