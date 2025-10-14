using TMPro;
using UnityEngine;

[RequireComponent(typeof(StatProgressBar), typeof(StatUpgradeController))]
public class StatsType : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;

    protected Stats _statsType;
    private StatProgressBar _progressBar;
    private StatUpgradeController _upgradeController;

    public Stats StatType => _statsType;

    private void Awake()
    {
        _progressBar = GetComponent<StatProgressBar>();
        _upgradeController = GetComponent<StatUpgradeController>();
    }

    private void OnDestroy()
    {
        if (StatsManager.Instance != null)
            StatsManager.Instance.OnUpdateUIStats -= UpdateUI;
    }

    public virtual void Initialize()
    {
        if (StatsManager.Instance != null)
            StatsManager.Instance.OnUpdateUIStats += UpdateUI;

        _upgradeController.Initialize(_statsType);
        _progressBar.Initialize(_statsType);

        UpdateUI(_statsType, GetCurrentStatValue());
    }

    private void UpdateUI(Stats stat, int value)
    {
        if (stat != _statsType) return;

        if (_upgradeController.IsMaxLevelReached())
        {
            ShowMaxLevel();
        }
        else
        {
            _text.text = value.ToString();
            _progressBar.UpdateProgressSmooth(value);

            if (_upgradeController.CheckLevelUp(value))
            {
                _progressBar.LevelUp();
            }
        }
    }

    private void ShowMaxLevel()
    {
        _text.text = "MAX";
        _progressBar.SetMaxProgress();
    }

    private int GetCurrentStatValue()
    {
        var character = Progress.Instance?.PlayerInfo?.CurrentCharacter;
        return character == null ? 0 : _statsType switch
        {
            Stats.Balks => character.Balk,
            Stats.Bench => character.Bench,
            Stats.HorizontalBar => character.HorizontalBars,
            Stats.Foots => character.Foots,
            _ => 0
        };
    }
}