
public class OpenedCharacterManagerCondition : IInitializationCondition
{
    public string ConditionName => "OpenedCharacterManager";

    public bool IsConditionMet()
    {
        return OpenedCharactersManager.Instance != null;
    }
}
