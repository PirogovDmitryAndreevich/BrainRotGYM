using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatsType : MonoBehaviour
{
    private const float UpgradeCoefficient = 1000f;

    [SerializeField] protected TextMeshProUGUI _text;
    [SerializeField] protected Image _progressBar;

    protected Stats _statsType;
    protected bool _isInitialized;

    public Stats StatType => _statsType;

    protected virtual void OnDestroy()
    {
        StatsManager.Instance.OnUpdateUIStats -= UpdateUI;
    }

    public virtual void Initialize()
    {
        if (_isInitialized) return;

        if (StatsManager.Instance != null)
            StatsManager.Instance.OnUpdateUIStats += UpdateUI;

        _isInitialized = true;
    }

    protected virtual void UpdateUI(Stats stat, int value)
    {


        if (stat == _statsType)
        {
            _text.text = value.ToString();
            UpdateProgressBar(value);
        }
    }

    protected void UpdateProgressBar(int value)
    {
        float remainder = value % UpgradeCoefficient;
        _progressBar.fillAmount = remainder / UpgradeCoefficient;
    }
}
