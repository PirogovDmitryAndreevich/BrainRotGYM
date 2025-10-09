using UnityEngine;
using UnityEngine.UI;

[RequireComponent (typeof(Button))]
public class TestOpenCharactersButton : MonoBehaviour
{
    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        CharactersMenu.Instance.OnOpenNewCharacter?.Invoke(CharactersEnum.TrallalleroTrallallaRed);
        CharactersMenu.Instance.OnOpenNewCharacter?.Invoke(CharactersEnum.TrallalleroTrallallaGreen);
    }
}
