using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class TrainingButtonBehaviour : MonoBehaviour
{
    protected Button _button;
    protected bool _isInitialized;
    protected Identificate _identificate;

    protected virtual void OnDestroy()
    {
        _button.onClick.RemoveAllListeners();
    }

    public virtual void Initialize(Identificate identificate)
    {
        if (_isInitialized) return;

        _button = GetComponent<Button>();
        _button.onClick.AddListener(OnClickButton);

        _identificate = identificate;

        _isInitialized = true;
    }

    protected virtual void OnClickButton()
    {
        StatsManager.Instance.AddStat?.Invoke(_identificate, GetCurrentLvlValue(_identificate));
    }

    private int GetCurrentLvlValue(Identificate statType)
    {
        if (Progress.Instance?.PlayerInfo == null) return 0;

        return statType switch
        {
            Identificate.Balks => Progress.Instance.PlayerInfo.LvlBalk,
            Identificate.Bench => Progress.Instance.PlayerInfo.LvlBench,
            Identificate.HorizontalBar => Progress.Instance.PlayerInfo.LvlHorizontalBars,
            Identificate.Foots => Progress.Instance.PlayerInfo.LvlFoots,
            _ => 0
        };
    }
}
