using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public abstract class UnlockCondition
{
    public abstract bool IsSatisfied(PlayerInfo player);
}
