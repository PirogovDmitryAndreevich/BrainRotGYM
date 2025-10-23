[System.Serializable]
public class LevelReachedCondition : UnlockCondition
{
    public int requiredLevel;
    public override bool IsSatisfied(CharacterProgressData characterProgress)
    {
        

        return true;
    }
}