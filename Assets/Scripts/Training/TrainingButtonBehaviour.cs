using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button), typeof(ShakeAreaEffect))]
public class TrainingButtonBehaviour : MonoBehaviour
{
    protected Button _button;
    protected bool _isInitialized;
    protected Identificate _identificate;
    protected ShakeAreaEffect _shakeAreaEffect;

    protected virtual void OnEnable()
    {
        if (MyPrefabs.Instance != null)
            MyPrefabs.Instance.SetValueInScorePrefab(GetCurrentLvlValue(_identificate));
    }

    protected virtual void OnDestroy()
    {
        _button.onClick.RemoveAllListeners();
    }

    public virtual void Initialize(Identificate identificate)
    {
        if (_isInitialized) return;

        _shakeAreaEffect = GetComponent<ShakeAreaEffect>();

        _button = GetComponent<Button>();
        _button.onClick.AddListener(OnClickButton);

        _identificate = identificate;

        _isInitialized = true;
    }

    protected virtual void OnClickButton()
    {
        StatsManager.Instance.AddStat?.Invoke(_identificate, GetCurrentLvlValue(_identificate));

        _shakeAreaEffect.Shake();
        FlyingUpScoreEffect.Instance.CreateClickUIEffect(Input.mousePosition);
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
