using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Condition_LevelReachedView : MonoBehaviour, IUnlockConditionView
{
    [SerializeField] private TextMeshProUGUI _conditionText;
    [SerializeField] private Image _charIcon;

    public void Initialize(UnlockCondition condition)
    {
        if (condition is LevelReachedCondition lvl)
        {
            _conditionText.text = lvl.RequiredLevel.ToString();
            _charIcon.sprite = CharacterDatabase.Instance.AllCharactersDictionary[lvl.TargetCharacterID].Icon;
        }
    }
}
