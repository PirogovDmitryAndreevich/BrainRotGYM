using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class Condition_LevelReachedView : ConditionView
{
    [SerializeField] private TextMeshProUGUI _conditionText;
    [SerializeField] private Image _charIcon;

    LevelReachedCondition _condition;
    PlayerInfo _player;

    public override void Initialize(UnlockCondition condition, PlayerInfo player)
    {
        _condition = condition as LevelReachedCondition;
        _player = player;

        if (_condition == null)
        {
            Debug.LogWarning($" Condition_LevelReachedView: ожидался тип LevelReachedCondition, получен {_condition.GetType().Name}");
            return;
        }
        _conditionText.text = _condition.RequiredLevel.ToString();
        _charIcon.sprite = CharacterDatabase.Instance.AllCharactersDictionary[_condition.TargetCharacterID].Icon;

        Subscribe();
        CheckCondition(_condition, _player);
    }

    private void Subscribe()
    {
        if (!OpenedCharactersManager.Instance.IsCharacterOpened(_condition.TargetCharacterID))        
            return;        
        else        
            OpenedCharactersManager.Instance.GetCharacterData(_condition.TargetCharacterID).OnLevelChanged
                += OnLevelTargetCharacterChanged;        
    }

    private void OnLevelTargetCharacterChanged() => CheckCondition(_condition, _player);
    private void OnDestroy() => OpenedCharactersManager.Instance.GetCharacterData(_condition.TargetCharacterID).OnLevelChanged
                -= OnLevelTargetCharacterChanged;
}
