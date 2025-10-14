using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button), typeof(Image))]
public class ButtonForCharacterInfoPanel : MonoBehaviour
{
    [SerializeField] private Stats _statType;
    [SerializeField] private Color _updateColor;

    private Button _button;
    private Image _image;
    private Color _defaultColor;
    private UpdateManager _updateManager;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _image = GetComponent<Image>();
        _defaultColor = _image.color;

        _button.interactable = false;
        _button.onClick.AddListener(OnClick);

        WaitForUpdateManager();
    }

    private void OnDestroy()
    {
        _button.onClick.RemoveAllListeners();

        if (_updateManager != null)
        {
            _updateManager.OnRequiresUpdateStatLvl -= ShowRequiresUpdate;
            _updateManager.OnStatsLvlUpdated -= HideShowUpdate;
        }
    }

    private void WaitForUpdateManager()
    {
        WaitingLoad.Instance.WaitAndExecute(
            () => UpdateManager.Instance != null,
            () =>
            {
                _updateManager = UpdateManager.Instance;
                _updateManager.OnRequiresUpdateStatLvl += ShowRequiresUpdate;
                _updateManager.OnStatsLvlUpdated += HideShowUpdate;
            }
        );
    }

    private void ShowRequiresUpdate(Stats stats)
    {
        Debug.LogWarning($"[ButtonForCharacterInfoPanel] {stats} требуется улучшение!");
        if (stats == _statType)
        {
            _button.interactable = true;
            _image.color = _updateColor;
        }
    }

    private void HideShowUpdate(Stats stats)
    {
        if (stats == _statType)
        {
            _button.interactable = false;
            _image.color = _defaultColor;
        }
    }

    private void OnClick()
    {
        Debug.Log($"[ButtonForCharacterInfoPanel] {_statType} button clicked - triggering update");
        _updateManager.OnUpdatingStatLvl?.Invoke(_statType);
    }
}