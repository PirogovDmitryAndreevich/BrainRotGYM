using UnityEngine;

public class OpenedCharacterPanel : MonoBehaviour
{
    [SerializeField] private Transform _layoutGroup;

    private void Awake()
    {
        GameManager.Instance.OnAllSystemsReady += Initialize;
    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.OnAllSystemsReady -= Initialize;
    }

    private void Initialize()
    {
        if (OpenedCharactersManager.Instance.IsLoadIsComplete)
            FillContent();
        else
            WaitingLoad.Instance.WaitAndExecute
                (
                    () => OpenedCharactersManager.Instance.IsLoadIsComplete == true,
                    () => FillContent()
                );
    }

    private void FillContent()
    {
        foreach (var character in CharacterDatabase.Instance.AllCharactersDictionary)
        {
            GameObject openedCharacter = Instantiate(MyPrefabs.Instance.SelectCharacterButton, _layoutGroup);

            SelectCharacterButtonComponent component = openedCharacter.GetComponent<SelectCharacterButtonComponent>();
            component.InitializeCharacterButtonSelect(CharacterDatabase.Instance.AllCharactersDictionary[character.Key].CharacterID);
        }
    }
}
