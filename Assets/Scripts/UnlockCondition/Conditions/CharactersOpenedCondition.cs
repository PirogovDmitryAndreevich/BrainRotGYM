using UnityEngine;

public class CharactersOpenedCondition : UnlockCondition
{
    public int TargetNumberOfCharacters;

    public override bool IsSatisfied(PlayerInfo player)
    {
        return player.OpenedCharacters.Count >= TargetNumberOfCharacters;
    }
}
