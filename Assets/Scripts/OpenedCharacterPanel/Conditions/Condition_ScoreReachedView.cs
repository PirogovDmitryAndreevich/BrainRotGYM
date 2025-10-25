using TMPro;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Condition_ScoreReachedView : ConditionView
{
    [SerializeField] private TextMeshProUGUI _targetScore;

    ScoreReachedCondition _condition;
    PlayerInfo _player;

    public override void Initialize(UnlockCondition condition, PlayerInfo player)
    {
        _condition = condition as ScoreReachedCondition;
        _player = player;

        if (_condition == null)
        {
            Debug.LogWarning($"Condition_ScoreReachedView: ожидался ScoreReachedCondition, получен {_condition.GetType().Name}");
            return;
        }

        _targetScore.text = _condition.TargetScore.ToString();

        CheckCondition(_condition, _player);
        _player.OnScoreChanged += OnPlayerScoreChanged;
    }

    private void OnPlayerScoreChanged() => CheckCondition(_condition, _player);
    private void OnDestroy() => _player.OnScoreChanged -= OnPlayerScoreChanged;
    
}
