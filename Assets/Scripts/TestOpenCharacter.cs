using UnityEngine;
using UnityEngine.UI;

[RequireComponent (typeof(Button))]
public class TestOpenCharacter : MonoBehaviour
{
    [SerializeField] private CharactersEnum _character;

    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        CharactersMenu.Instance.OnOpenNewCharacter(_character);
    }
}
