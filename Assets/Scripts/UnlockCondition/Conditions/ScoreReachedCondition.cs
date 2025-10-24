using UnityEngine;

public class ScoreReachedCondition : UnlockCondition
{
    public int TargetScore;

    public override bool IsSatisfied(PlayerInfo player)
    {
        return Progress.Instance.PlayerInfo.Score >= TargetScore;
    }
}
