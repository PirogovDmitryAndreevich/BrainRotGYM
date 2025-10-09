using System;
using System.Collections.Generic;
using UnityEngine;

public class CharactersMenu : MonoBehaviour
{
    public static CharactersMenu Instance;

    [SerializeField] public CharacterType[] _characters;

    public Action<CharactersEnum> OnSelectCharacter;
    public Action<CharactersEnum> OnOpenNewCharacter;
    public Action OnSelectedCharacter;

    private Dictionary<CharactersEnum, CharacterType> _charactersDictionary = new();
    private Dictionary<CharactersEnum, CharacterType> _openedCharacters = new();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            CreateDictionary();
            OnSelectCharacter += SelectCharacter;
            OnOpenNewCharacter += OpenNewCharacter;
        }
        else
        {
            Destroy(gameObject);
        }

        WaitingLoad.Instance.WaitAndExecute(
            () => Progress.Instance != null,
            () => LoadOpenedDictionary()
            );

    }

    private void OnDestroy()
    {
        OnSelectCharacter -= SelectCharacter;
        OnOpenNewCharacter -= OpenNewCharacter;
    }

    private void SelectCharacter(CharactersEnum id)
    {
        if (!_openedCharacters.ContainsKey(id))
        {
            Debug.Log($"{id} has not opened");
            return;
        }

        Progress.Instance.PlayerInfo.CurrentCharacter = _openedCharacters[id];
        Progress.Instance.Save();
        Debug.Log($"CurrentCharacter = {id}");
        OnSelectedCharacter?.Invoke();
    }

    private void OpenNewCharacter(CharactersEnum id)
    {
        if (!_charactersDictionary.ContainsKey(id))
        {
            Debug.LogError($"Character {id} not found in dictionary!");
            return;
        }

        if (_openedCharacters.ContainsKey(id))
        {
            Debug.LogWarning($"Character {id} is already opened!");
            return;
        }

        var newCharacter = new CharacterType(_charactersDictionary[id]);
        Progress.Instance.PlayerInfo.OpenedCharacters.Add(newCharacter);
        Progress.Instance.Save();
        LoadOpenedDictionary();

        Debug.Log($"New character opened: {id}");
    }

    private void CreateDictionary()
    {
        foreach (var character in _characters)
        {
            if (!_charactersDictionary.ContainsKey(character.CharacterID))
            {
                _charactersDictionary[character.CharacterID] = character;
            }
        }

        Debug.Log($"Characters dictionary created with {_charactersDictionary.Count} characters");
    }

    private void LoadOpenedDictionary()
    {
        _openedCharacters.Clear();

        if (Progress.Instance.PlayerInfo.OpenedCharacters.Count == 0)
        {
            OpenNewCharacter(CharactersEnum.TrallalleroTrallalla);

            WaitingLoad.Instance.WaitAndExecute
                (
                    () => ShowManager.Instance != null,
                    () => SelectCharacter(CharactersEnum.TrallalleroTrallalla)
                );            
        }

        foreach (var character in Progress.Instance.PlayerInfo.OpenedCharacters)
        {
            if (character != null && !_openedCharacters.ContainsKey(character.CharacterID))
            {
                _openedCharacters[character.CharacterID] = character;
            }
        }

        SelectCharacter(Progress.Instance.PlayerInfo.CurrentCharacter.CharacterID);
        Debug.Log($"Opened characters loaded: {_openedCharacters.Count}");
    }
}
