public class MiniGamesCondition : IInitializationCondition
{
    public string ConditionName => "MiniGames";

    public bool IsConditionMet()
    {
        return MiniGamesManager.Instance != null;
    }

}
