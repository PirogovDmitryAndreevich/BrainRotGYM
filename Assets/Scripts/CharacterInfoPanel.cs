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
                () => CharactersDataManager.Instance != null,
                () =>
                {
                    CharactersDataManager.Instance.OnSelectedCharacter += SetInform;
                    SetInform();
                }
            );
    }

    private void OnDestroy()
    {
        CharactersDataManager.Instance.OnSelectedCharacter -= SetInform;
    }

    private void SetInform()
    {
        if (CharactersDataManager.Instance != null)
        {
            WaitingLoad.Instance.WaitAndExecute
                (
                    () => CharactersDataManager.Instance.CurrentCharacterView != null,
                    () =>
                    {
                        _name.text = CharactersDataManager.Instance.CurrentCharacterView.Name;
                        _icon.sprite = CharactersDataManager.Instance.CurrentCharacterView.Icon;
                    }
                );
        }
        else
        {
            Debug.Log($"SetInform: CurrentCharacter is not loaded");
        }
    }
}
