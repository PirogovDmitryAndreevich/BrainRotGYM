using TMPro;
using UnityEngine;

public class Condition_CharactersOpenedView : MonoBehaviour, IUnlockConditionView
{
    [SerializeField] private TextMeshProUGUI _targetNumberOfCharacters;

    public void Initialize(UnlockCondition condition)
    {
        if (condition is CharactersOpenedCondition chars)
        {
            _targetNumberOfCharacters.text = chars.TargetNumberOfCharacters.ToString();
        }
    }
}
