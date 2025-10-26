using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button), typeof(Outline))]
public class SelectCharacterButtonComponent : MonoBehaviour, ISelectableCharacter
{
    private CharacterProgressData _character;
    private CharacterData _characterView;
    private CharactersDataManager _charactersDataManager;
    private PlayerInfo _playerInfo;

    private Button _button;
    private bool _isSelected;
    private CharactersEnum _charactersID;

    [Header("Common UI")]
    [SerializeField] private Image _icon;
    [SerializeField] private Image _background;
    [SerializeField] private Outline _outline;
    [SerializeField] private Image _backgroundIcon;
    private Color _defaultBackgroundColor;

    [Header("Stats UI")]
    [SerializeField] private GameObject _unlockContainer;
    [SerializeField] private TextMeshProUGUI _balks;
    [SerializeField] private TextMeshProUGUI _bench;
    [SerializeField] private TextMeshProUGUI _horizontalBar;
    [SerializeField] private TextMeshProUGUI _foots;
    [SerializeField] private TextMeshProUGUI _level;
    [SerializeField] private Color _selectColor;

    [Header("Locked UI")]
    [SerializeField] private GameObject _lockedContainer;
    [SerializeField] private ConditionViewFactory _conditionViewFactory;
    [SerializeField] private Transform _conditionsGridContainer;
    [SerializeField] private Color _lockColorBG;

    private int _conditionsCount;
    private int _conditionsCompletedCount;
    private Color _defaultColor;

    public CharactersEnum CharacterID => _charactersID;

    private readonly List<IUnlockConditionView> _spawnedViews = new();

    private void OnDestroy()
    {
        foreach (var view in _spawnedViews)
        {
            if (view != null)
                view.OnConditionCompleted -= HandleConditionCompleted;
        }

        _spawnedViews.Clear();
    }

    public void InitializeCharacterButtonSelect(CharactersEnum characterID)
    {
        _charactersID = characterID;
        _character = OpenedCharactersManager.Instance.GetCharacterData(_charactersID);
        _characterView = CharacterDatabase.Instance.GetCharacterData(_charactersID);
        _charactersDataManager = CharactersDataManager.Instance;
        _playerInfo = Progress.Instance.PlayerInfo;

        _button = GetComponent<Button>();
        _outline = GetComponent<Outline>();

        _backgroundIcon.color = _characterView.SecondaryColor;
        _icon.sprite = _characterView.Icon;
        _defaultBackgroundColor = _background.color;
        _defaultColor = _outline.effectColor;

        _conditionsCount = _characterView.unlockConditions.Count;
        _conditionsCompletedCount = 0;

        if (OpenedCharactersManager.Instance.IsCharacterOpened(_charactersID))
            UnlockCharacter();
        else
            BlockCharacter();
    }

    // ------------------------ Selection ------------------------

    public void Deselect()
    {
        _isSelected = false;
        _outline.effectColor = _defaultColor;
    }

    public void Select()
    {
        _isSelected = true;
        _outline.effectColor = _selectColor;
    }

    // ------------------------ Unlocked State ------------------------

    private void UnlockCharacter()
    {
        _lockedContainer.SetActive(false);
        _unlockContainer.SetActive(true);

        _background.color = _defaultBackgroundColor;

        _balks.text = _character.LvlBalk.ToString();
        _bench.text = _character.LvlBench.ToString();
        _horizontalBar.text = _character.LvlHorizontalBars.ToString();
        _foots.text = _character.LvlFoots.ToString();
        _level.text = _character.Level.ToString();

        _button.onClick.RemoveAllListeners();
        _button.onClick.AddListener(() =>
        {
            CharacterSelectionController.Instance.SelectCharacter(this);
        });
        _button.interactable = true;

        Deselect();

        if (Progress.Instance.PlayerInfo.CurrentCharacter.CharacterID == _charactersID)
            Select();
    }

    // ------------------------ Locked State ------------------------

    private void BlockCharacter()
    {
        _unlockContainer.SetActive(false);
        _lockedContainer.SetActive(true);

        _background.color = _lockColorBG;
        _button.interactable = false;

        foreach (Transform child in _conditionsGridContainer)
            Destroy(child.gameObject);

        _spawnedViews.Clear();

        foreach (var condition in _characterView.unlockConditions)
        {
            var prefab = _conditionViewFactory.GetPrefabForCondition(condition);
            if (prefab == null)
            {
                Debug.LogWarning($"Не найден префаб для условия {condition.GetType().Name}");
                continue;
            }

            var instance = Instantiate(prefab, _conditionsGridContainer);
            if (instance.TryGetComponent<IUnlockConditionView>(out var view))
            {
                _spawnedViews.Add(view);
                view.OnConditionCompleted += HandleConditionCompleted;

                view.Initialize(condition, _playerInfo);

                if (view.IsCompleted)
                    _conditionsCompletedCount++;
            }
        }

        TryUnlockCharacter();

        _button.onClick.RemoveAllListeners();
        _button.interactable = false;
    }

    private void HandleConditionCompleted(IUnlockConditionView view)
    {
        _conditionsCompletedCount++;
        TryUnlockCharacter();
    }

    private void CanOpenNewCharacter()
    {
        _background.color = _selectColor;

        _button.onClick.RemoveAllListeners();
        _button.onClick.AddListener(() =>
        {
            _charactersDataManager.OnOpenNewCharacter?.Invoke(CharacterID);
            UnlockCharacter();
        });
        _button.interactable = true;
    }   

    private void TryUnlockCharacter()
    {
        if (_conditionsCompletedCount >= _conditionsCount)
        {
            Debug.Log($" Все условия для {_charactersID} выполнены!");
            CanOpenNewCharacter();
        }
    }
}
