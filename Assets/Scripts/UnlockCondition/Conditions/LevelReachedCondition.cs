using System;

[Serializable]
public class LevelReachedCondition : UnlockCondition
{
    public CharactersEnum TargetCharacterID;
    public int RequiredLevel;

    public override bool IsSatisfied(PlayerInfo player)
    {
        var targetProgress = player.OpenedCharacters.Find(c => c.CharacterID == TargetCharacterID);

        return targetProgress != null && targetProgress.Level >= RequiredLevel;
    }
}
