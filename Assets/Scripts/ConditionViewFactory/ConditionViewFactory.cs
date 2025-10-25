using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/UnlockCondition/ConditionViewFactory")]
public class ConditionViewFactory : ScriptableObject
{
    [Serializable]
    public class Pair
    {
        public string conditionTypeName;
        public GameObject prefab;
    }

    [SerializeField] private List<Pair> pairs = new List<Pair>();

    public GameObject GetPrefabForCondition(UnlockCondition condition)
    {
        if (condition == null) return null;
        var name = condition.GetType().Name;
        var p = pairs.Find(x => x.conditionTypeName == name);
        return p?.prefab;
    }
}
