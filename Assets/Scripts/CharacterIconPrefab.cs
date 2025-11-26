using UnityEngine;
using UnityEngine.UI;

public class CharacterIconPrefab : MonoBehaviour
{
    [SerializeField] private Image _icon;

    public void SetIcon(CharactersEnum character)
    {
        _icon.sprite = CharacterDatabase.Instance.GetCharacterData(character).Icon;
    }
}
