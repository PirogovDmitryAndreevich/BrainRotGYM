using UnityEngine;

[System.Serializable]
public abstract class UnlockCondition
{
    public abstract bool IsSatisfied(CharacterProgressData characterProgress);
}
