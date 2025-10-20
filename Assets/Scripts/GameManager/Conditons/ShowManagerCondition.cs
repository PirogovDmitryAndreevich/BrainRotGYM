public class ShowManagerCondition : IInitializationCondition
{
    public string ConditionName => "ShowManager and all scene.";

    private readonly int _expectedSceneCount;

    public ShowManagerCondition(int expectedSceneCount)
    {
        _expectedSceneCount = expectedSceneCount;
    }

    public bool IsConditionMet()
    {
        return ShowScenesManager.Instance != null &&
            ShowScenesManager.Instance.Scenes != null &&
            ShowScenesManager.Instance.Scenes.Count >= _expectedSceneCount;
    }
}
