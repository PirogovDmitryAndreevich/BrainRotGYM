public class CharacterDatabaseCondition : IInitializationCondition
{
    public string ConditionName => "CharacterDatabase";

    public bool IsConditionMet()
    {
        return CharacterDatabase.Instance != null;
    }
}
