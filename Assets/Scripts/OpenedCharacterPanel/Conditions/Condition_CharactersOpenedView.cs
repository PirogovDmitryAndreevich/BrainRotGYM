using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class Condition_CharactersOpenedView : ConditionView
{
    [SerializeField] private TextMeshProUGUI _targetNumberOfCharacters;

    CharactersOpenedCondition _condition;
    PlayerInfo _player;

    public override void Initialize(UnlockCondition condition, PlayerInfo player)
    {
        _condition = condition as CharactersOpenedCondition;
        _player = player;

        if (_condition == null)
        {
            Debug.LogWarning($" Condition_CharactersOpenedView: ожидался тип CharactersOpenedCondition, получен {_condition.GetType().Name}");
            return;
        }

        _targetNumberOfCharacters.text = _condition.TargetNumberOfCharacters.ToString();

        CharactersDataManager.Instance.OnNewCharacterIsOpened += OpenedNewCharacter;
        CheckCondition(_condition, _player);
    }

    private void OpenedNewCharacter(CharactersEnum character) => CheckCondition(_condition, _player);
    private void OnDestroy() => CharactersDataManager.Instance.OnNewCharacterIsOpened -= OpenedNewCharacter;

}
