using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectionController : MonoBehaviour
{
    public static CharacterSelectionController Instance { get; private set; }

    [SerializeField] private Button _confirmButton;

    private Image _confirmImage;

    private ISelectableCharacter _currentSelection;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        if (_confirmButton != null)
        {
            _confirmButton.interactable = false;
            _confirmImage = _confirmButton.GetComponent<Image>();
            _confirmButton.onClick.AddListener(ConfirmSelection);
        }
    }

    public void SelectCharacter(ISelectableCharacter character)
    {
        // снимаем выделение со старого
        if (_currentSelection != null && _currentSelection != character)
            _currentSelection.Deselect();

        // выбираем нового
        _currentSelection = character;
        _currentSelection.Select();

        // активируем кнопку подтверждени€
        if (_confirmButton != null)
        {
            _confirmImage.color = Color.green;
            _confirmButton.interactable = true;
        }
    }

    private void ConfirmSelection()
    {
        if (_currentSelection == null) return;

        // подтверждаем выбор
        CharactersDataManager.Instance.OnSelectCharacter?.Invoke(_currentSelection.CharacterID);

        // опционально Ч сбрасываем выделение
        _confirmButton.interactable = false;
        _confirmImage.color = Color.white;
    }
}
