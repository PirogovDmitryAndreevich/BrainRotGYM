using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class SwitchCharacterButton : MonoBehaviour
{
    [SerializeField] private CharactersEnum character;

    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(OnClick);
    }

    private void OnDestroy()
    {
        _button.onClick.RemoveAllListeners();
    }

    private void OnClick()
    {
        if (CharactersDataManager.Instance != null)
            CharactersDataManager.Instance.OnSelectCharacter(character);
    }
}
