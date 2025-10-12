using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button), typeof(ResistanceProgressBar))]
public class TrainingButtonBehaviour : MonoBehaviour
{
    protected Button _button;
    protected bool _isInitialized;
    protected Identificate _identifier;
    protected ResistanceProgressBar _progressBar;

    private const int DefaultValueToAddStat = 1;
    protected int _valueToAddStat = 1;

    protected virtual void OnEnable()
    {        
        WaitingLoad.Instance.WaitAndExecute(
            () => Progress.Instance?.PlayerInfo?.CurrentCharacter != null,
            () => {
                if (_progressBar != null)
                {
                    _progressBar.Initialize(GetCurrentLvlValue(_identifier));
                    _progressBar.OnProgressBarIsCompleted += MultiplierForValueToAddStat;
                    _progressBar.OnProgressBarIsReset += ResetMultiplier;
                }
            }
        );
    }

    protected virtual void OnDestroy()
    {        
        _button.onClick.RemoveAllListeners();
        //_progressBar.OnProgressBarIsCompleted -=
    }

    public virtual void Initialize(Identificate identifier)
    {
        if (_isInitialized) return;

        _identifier = identifier;
        _progressBar = GetComponent<ResistanceProgressBar>();
        _button = GetComponent<Button>();

        if (Progress.Instance?.PlayerInfo?.CurrentCharacter != null)
        {
            _progressBar.Initialize(GetCurrentLvlValue(_identifier));
        }
        else
        {
            WaitingLoad.Instance.WaitAndExecute(
                () => Progress.Instance?.PlayerInfo?.CurrentCharacter != null,
                () => _progressBar.Initialize(GetCurrentLvlValue(_identifier))
            );
        }

        //_progressBar.OnProgressBarIsCompleted +=

        _button.onClick.AddListener(OnClickButton);

        _isInitialized = true;
    }

    protected virtual void OnClickButton()
    {        
        if (Progress.Instance?.PlayerInfo?.CurrentCharacter == null)
        {
            Debug.LogWarning("CurrentCharacter is null, cannot add stats");
            return;
        }

        if (_progressBar == null)
        {
            Debug.LogError("ProgressBar is not initialized!");
            return;
        }        

        if (StatsManager.Instance != null)
            StatsManager.Instance.OnAddStat?.Invoke(_identifier, _valueToAddStat);
        else
            Debug.LogError("StatsManager.Instance is null!");

        _progressBar.OnButtonClick();

        if (ShakeAreaEffect.Instance != null) ShakeAreaEffect.Instance.Shake();
        else  Debug.LogWarning("ShakeAreaEffect.Instance is null");

        if (FlyingUpScoreEffect.Instance != null)  FlyingUpScoreEffect.Instance.CreateClickUIEffect(Input.mousePosition, _valueToAddStat);
        else  Debug.LogWarning("FlyingUpScoreEffect.Instance is null");
    }

    private void MultiplierForValueToAddStat()
    {
        int multiplier = 2;

        _valueToAddStat *= multiplier;
    }

    private void ResetMultiplier()
    {
        _valueToAddStat = DefaultValueToAddStat;
    }

    private int GetCurrentLvlValue(Identificate statType)
    {
        if (Progress.Instance?.PlayerInfo?.CurrentCharacter == null)
        {
            Debug.LogWarning("CurrentCharacter is null in GetCurrentLvlValue");
            return 1;
        }

        return statType switch
        {
            Identificate.Balks => Progress.Instance.PlayerInfo.CurrentCharacter.LvlBalk,
            Identificate.Bench => Progress.Instance.PlayerInfo.CurrentCharacter.LvlBench,
            Identificate.HorizontalBar => Progress.Instance.PlayerInfo.CurrentCharacter.LvlHorizontalBars,
            Identificate.Foots => Progress.Instance.PlayerInfo.CurrentCharacter.LvlFoots,
            _ => 150
        };
    }
}
