using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Condition_ScoreReachedView : MonoBehaviour, IUnlockConditionView
{
    [SerializeField] private TextMeshProUGUI _targetScore;

    public void Initialize(UnlockCondition condition)
    {
        if (condition is ScoreReachedCondition score)
        {
            _targetScore.text = score.TargetScore.ToString();
        }
    }
}
