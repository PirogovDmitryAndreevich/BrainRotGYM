public class CharacterDataManagerCondition : IInitializationCondition
{
    public string ConditionName => "CharacterDataManager";

    public bool IsConditionMet()
    {
        return CharactersDataManager.Instance != null;
    }
}
