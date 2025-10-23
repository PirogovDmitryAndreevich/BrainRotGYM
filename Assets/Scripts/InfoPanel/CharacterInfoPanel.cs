using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterInfoPanel : StatDataHelper
{
    [Header("Text settings")]
    [SerializeField] private TextMeshProUGUI _levelTotal;
    [SerializeField] private TextMeshProUGUI _lvlBalks;
    [SerializeField] private TextMeshProUGUI _lvlBench;
    [SerializeField] private TextMeshProUGUI _lvlHorizontalBar;
    [SerializeField] private TextMeshProUGUI _lvlFoot;
    [SerializeField] private Button _lvlUpButton;
    [SerializeField] private Image _icon;

    private CharacterData _currentCharacterView;
    private CharacterProgressData _currentCharacterData;

    private void Awake()
    {
        _lvlUpButton.gameObject.SetActive(false);

        GameManager.Instance.OnAllSystemsReady += Initialize;
        if (GameManager.Instance.IsAllSystemsReady)
            Initialize();

        _lvlUpButton.onClick.AddListener(LvlUpdateButtonOnClick);
    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.OnAllSystemsReady -= Initialize;

        if (CharactersDataManager.Instance != null)
            CharactersDataManager.Instance.OnSelectedCharacter -= ChangeCharacter;

        if (UpdateManager.Instance != null)
        {
            UpdateManager.Instance.OnRequiresUpdateLevel -= ShowRequiresUpdateLevel;
            UpdateManager.Instance.OnLevelUpdated -= SetLevel;
            UpdateManager.Instance.OnStatsLvlUpdated -= SetStats;
            UpdateManager.Instance.OnLevelUpdated -= HideUpdateLevel;
        }

        _lvlUpButton.onClick.RemoveAllListeners();
    }

    private void Initialize()
    {
        CharactersDataManager.Instance.OnSelectedCharacter += ChangeCharacter;
        UpdateManager.Instance.OnRequiresUpdateLevel += ShowRequiresUpdateLevel;
        UpdateManager.Instance.OnLevelUpdated += SetLevel;
        UpdateManager.Instance.OnStatsLvlUpdated += SetStats;
        UpdateManager.Instance.OnLevelUpdated += HideUpdateLevel;

        _currentCharacterView = CharactersDataManager.Instance.CurrentCharacterView;

        if (_currentCharacterView != null)
            ChangeCharacter();
    }

    private void ChangeCharacter()
    {
        _currentCharacterView = CharactersDataManager.Instance.CurrentCharacterView;
        _currentCharacterData = Progress.Instance.PlayerInfo.CurrentCharacter;
        SetInform();
    }

    private void SetInform()
    {
        _icon.sprite = _currentCharacterView.Icon;
        foreach (Stats stat in Enum.GetValues(typeof(Stats)))
            SetStats(stat);
        SetLevel();
    }

    private void SetStats(Stats stats)
    {
        string levelText = GetCurrentStatLevel(stats).ToString();
        switch (stats)
        {
            case Stats.Balks: _lvlBalks.text = levelText; break;
            case Stats.Bench: _lvlBench.text = levelText; break;
            case Stats.HorizontalBar: _lvlHorizontalBar.text = levelText; break;
            case Stats.Foots: _lvlFoot.text = levelText; break;
        }
    }

    private void SetLevel() => _levelTotal.text = _currentCharacterData.Level.ToString();

    private void ShowRequiresUpdateLevel()
    {
        _lvlUpButton.gameObject.SetActive(true);
        Debug.Log("[CharacterInfoPanel] Level up button shown - requires update");
    }

    private void HideUpdateLevel() => _lvlUpButton.gameObject.SetActive(false);

    private void LvlUpdateButtonOnClick() => UpdateManager.Instance.TryUpdateLevel();
}