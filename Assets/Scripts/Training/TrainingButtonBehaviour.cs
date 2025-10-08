using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button), typeof(ResistanceProgressBar))]
public class TrainingButtonBehaviour : MonoBehaviour
{
    protected Button _button;
    protected bool _isInitialized;
    protected Identificate _identificate;
    protected ResistanceProgressBar _progressBar;

    protected virtual void OnEnable()
    {
        if (MyPrefabs.Instance != null)
            MyPrefabs.Instance.SetValueInScorePrefab(GetCurrentLvlValue(_identificate));

        if (Progress.Instance != null && _progressBar != null)
            _progressBar.Initialize(GetCurrentLvlValue(_identificate));
    }

    protected virtual void OnDestroy()
    {        
        _button.onClick.RemoveAllListeners();
        //_progressBar.OnProgressBarIsCompleted -=
    }

    public virtual void Initialize(Identificate identificate)
    {
        if (_isInitialized) return;

        _identificate = identificate;
        _progressBar = GetComponent<ResistanceProgressBar>();
        _button = GetComponent<Button>();

        if (Progress.Instance != null)        
            _progressBar.Initialize(GetCurrentLvlValue(_identificate));
        
        //_progressBar.OnProgressBarIsCompleted +=

        _button.onClick.AddListener(OnClickButton);

        _isInitialized = true;
    }

    protected virtual void OnClickButton()
    {
        if (StatsManager.Instance != null)
            StatsManager.Instance.OnAddStat?.Invoke(_identificate, GetCurrentLvlValue(_identificate));

        _progressBar.OnButtonClick();

        if (ShakeAreaEffect.Instance != null) ShakeAreaEffect.Instance.Shake();
        else  Debug.LogWarning("ShakeAreaEffect.Instance is null");

        if (FlyingUpScoreEffect.Instance != null)  FlyingUpScoreEffect.Instance.CreateClickUIEffect(Input.mousePosition);
        else  Debug.LogWarning("FlyingUpScoreEffect.Instance is null");
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
