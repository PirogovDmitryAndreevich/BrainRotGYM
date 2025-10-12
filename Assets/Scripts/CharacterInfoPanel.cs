using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterInfoPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _name;
    [SerializeField] private Image _icon;

    private void Awake()
    {
        WaitingLoad.Instance.WaitAndExecute
            (   
                () => CharactersMenu.Instance != null,
                () =>
                {
                    CharactersMenu.Instance.OnSelectedCharacter += SetInform;
                    SetInform();
                }
            );
    }

    private void OnDestroy()
    {
        CharactersMenu.Instance.OnSelectedCharacter -= SetInform;
    }

    private void SetInform()
    {
        if (Progress.Instance.PlayerInfo.CurrentCharacter != null)
        {
            _name.text = Progress.Instance.PlayerInfo.CurrentCharacter.Name;
            _icon.sprite = Progress.Instance.PlayerInfo.CurrentCharacter.Icon;
        }
        else
        {
            Debug.Log($"SetInform: CurrentCharacter is not loaded");
        }
    }
}
