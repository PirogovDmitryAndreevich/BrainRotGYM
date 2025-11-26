using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class SelectCharacterButtonComponent : MonoBehaviour, ISelectableCharacter
{
    private CharacterProgressData _character;
    private CharacterData _characterView;
    private CharactersDataManager _charactersDataManager;
    private PlayerInfo _playerInfo;

    private Button _button;
    private CharactersEnum _charactersID;

    [Header("Common UI")]
    [SerializeField] private Image _icon;
    [SerializeField] private Image _background;
    [SerializeField] private Image _selectFrame;

    [Header("Front ground")]
    [SerializeField] private Image _frontground;
    [SerializeField] private Image _ground;
    [SerializeField] private Image _gradient;

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

    private int _conditionsCount;
    private int _conditionsCompletedCount;

    public CharactersEnum CharacterID => _charactersID;

    private readonly List<IUnlockConditionView> _spawnedViews = new();

    private void Awake()
    {
        _button = GetComponent<Button>();
        _selectFrame.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        foreach (var view in _spawnedViews)
        {
            if (view != null)
                view.OnConditionCompleted -= HandleConditionCompleted;
        }

        _spawnedViews.Clear();
    }

    public void InitializeCharacterButtonSelect(CharactersEnum characterID, Action<ISelectableCharacter> onSelectAction)
    {
        _charactersID = characterID;
        _character = OpenedCharactersManager.Instance.GetCharacterData(_charactersID);
        _characterView = CharacterDatabase.Instance.GetCharacterData(_charactersID);
        _charactersDataManager = CharactersDataManager.Instance;
        _playerInfo = Progress.Instance.PlayerInfo;
        _icon.sprite = _characterView.Icon;
        _background.color = _characterView.MainColor;

        _ground.color = _characterView.MainColor;
        _gradient.color = _characterView.MainColor;
        _frontground.color = new Color(1f, 1f, 1f, 0f);

        _conditionsCount = _characterView.unlockConditions.Count;
        _conditionsCompletedCount = 0;

        _button.onClick.RemoveAllListeners();
        _button.onClick.AddListener(() => onSelectAction.Invoke(this));


        if (OpenedCharactersManager.Instance.IsCharacterOpened(_charactersID))
            UnlockCharacter();
        else
            BlockCharacter();
    }

    // ------------------------ Selection ------------------------

    public void Deselect() => _selectFrame.gameObject.SetActive(false);
    public void Select() => _selectFrame.gameObject.SetActive(true);

    // ------------------------ Unlocked State ------------------------

    private void UnlockCharacter()
    {
        _lockedContainer.SetActive(false);
        _unlockContainer.SetActive(true);

        _background.color = _characterView.MainColor;

        _ground.color = _characterView.MainColor;
        _gradient.color = _characterView.MainColor;
        _frontground.color = new Color(1f, 1f, 1f, 0f);

        _balks.text = _character.LvlBalk.ToString();
        _bench.text = _character.LvlBench.ToString();
        _horizontalBar.text = _character.LvlHorizontalBars.ToString();
        _foots.text = _character.LvlFoots.ToString();
        _level.text = _character.Level.ToString();

        _button.interactable = true;

        Deselect();        
    }

    // ------------------------ Locked State ------------------------

    private void BlockCharacter()
    {
        _unlockContainer.SetActive(false);
        _lockedContainer.SetActive(true);

        _background.color = _characterView.MainColor;

        _ground.color = _characterView.MainColor;
        _gradient.color = _characterView.MainColor;
        _frontground.color = new Color(0f, 0f, 0f, 0.5f);

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

        _button.onClick.RemoveAllListeners();
        _button.interactable = false;

        TryUnlockCharacter();
    }

    private void HandleConditionCompleted(IUnlockConditionView view)
    {
        _conditionsCompletedCount++;
        TryUnlockCharacter();
    }

    private void CanOpenNewCharacter()
    {
        _background.color = _selectColor;

        _ground.color = _selectColor;
        _gradient.color = _selectColor;
        _frontground.color = new Color(0f, 0f, 0f, 0f);

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
            CanOpenNewCharacter();

    }
}
