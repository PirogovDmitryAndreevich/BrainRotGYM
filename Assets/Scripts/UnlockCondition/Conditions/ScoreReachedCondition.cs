using UnityEngine;

public class ScoreReachedCondition : UnlockCondition
{
    public int TargetScore;

    public override bool IsSatisfied(PlayerInfo player)
    {
        return player.Score >= TargetScore;
    }
}
