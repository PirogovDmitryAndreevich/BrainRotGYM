using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button), typeof(Outline))]
public class SelectCharacterButtonComponent : MonoBehaviour, ISelectableCharacter
{
    private CharacterProgressData _character;
    private CharacterData _characterView;
    private Button _button;
    private Outline _outline;
    private bool _isSelected;
    private CharactersEnum _charactersID;

    [SerializeField] private TextMeshProUGUI _balks;
    [SerializeField] private TextMeshProUGUI _bench;
    [SerializeField] private TextMeshProUGUI _horizontalBar;
    [SerializeField] private TextMeshProUGUI _foots;
    [SerializeField] private TextMeshProUGUI _level;

    [SerializeField] private Image _icon;
    [SerializeField] private Color _selectColor;
    private Color _defaultColor;

    public CharactersEnum CharacterID => _charactersID;

    public void InitializeCharacterButtonSelect(CharactersEnum characterID)
    {
        _charactersID = characterID;
        _character = OpenedCharactersManager.Instance.GetCharacterData(_charactersID);
        _characterView = CharacterDatabase.Instance.GetCharacterData(_charactersID);

        _balks.text = _character.LvlBalk.ToString();
        _bench.text = _character.LvlBench.ToString();
        _horizontalBar.text = _character.LvlHorizontalBars.ToString();
        _foots.text = _character.LvlFoots.ToString();
        _level.text = _character.Level.ToString();

        _icon.sprite = _characterView.Icon;

        _button = GetComponent<Button>();
        _outline = GetComponent<Outline>();
        _defaultColor = _outline.effectColor;

        _button.onClick.RemoveAllListeners();
        _button.onClick.AddListener(() =>
        {
            CharacterSelectionController.Instance.SelectCharacter(this);
        });

        Deselect();
    }

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
}
