using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharactersDataManager : MonoBehaviour
{
    public static CharactersDataManager Instance;

    [HideInInspector] public CharacterData CurrentCharacterView;

    public Action<CharactersEnum> OnSelectCharacter;
    public Action<CharactersEnum> OnOpenNewCharacter;
    public Action<CharactersEnum> OnNewCharacterIsOpened;
    public Action OnSelectedCharacter;

    private CharacterDatabase _database;
    private OpenedCharactersManager _openedCharacters;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        GameManager.Instance.OnAllSystemsReady += InitializeManagers;

        OnOpenNewCharacter += OpenNewCharacter;
        OnSelectCharacter += SelectCharacter;


    }

    private void OnDestroy()
    {
        OnOpenNewCharacter -= OpenNewCharacter;
        OnSelectCharacter -= SelectCharacter;
    }

    private void InitializeManagers()
    {
        _database = CharacterDatabase.Instance;
        _openedCharacters = OpenedCharactersManager.Instance;

        WaitingLoad.Instance.WaitAndExecute
            (
                () => _openedCharacters.IsLoadIsComplete = true,
                () => InitializeCharacters()
            );
    }

    private void InitializeCharacters()
    {
        if (Progress.Instance.PlayerInfo.OpenedCharacters.Count == 0)
        {
            OpenNewCharacter(CharactersEnum.TrallalleroTrallalla);
            SelectCharacter(CharactersEnum.TrallalleroTrallalla);
        }
        else
        {
            if (Progress.Instance.PlayerInfo.CurrentCharacter != null)
            {
                SelectCharacter(Progress.Instance.PlayerInfo.CurrentCharacter.CharacterID);
            }
            else
            {
                var currentCharacter = Progress.Instance.PlayerInfo.CurrentCharacter;
                var characterID = currentCharacter?.CharacterID ?? _openedCharacters.OpenedCharacters.Keys.First();
                SelectCharacter(characterID);
            }
        }

    }

    public void SelectCharacter(CharactersEnum characterID)
    {
        if (!_openedCharacters.IsCharacterOpened(characterID))
        {
            Debug.LogWarning($"Character {characterID} is not opened!");            
            return;
        }

        if (!_database.TryGetCharacterData(characterID, out var characterData))
        {
            Debug.LogError($"Character {characterID} not found in database!");
            return;
        }

        var character = _openedCharacters.GetCharacterData(characterID);
        CurrentCharacterView = characterData;
        Progress.Instance.PlayerInfo.CurrentCharacter = character;
        Progress.Instance.Save();

        OnSelectedCharacter?.Invoke();
    }

    public void OpenNewCharacter(CharactersEnum characterID)
    {
        if (_openedCharacters.IsCharacterOpened(characterID))
        {
            Debug.LogWarning($"Character {characterID} is already opened!");
            return;
        }

        if (!_database.ContainsCharacter(characterID))
        {
            Debug.LogError($"Character {characterID} not found in database!");
            return;
        }

        var characterProgress = new CharacterProgressData(characterID);

        _openedCharacters.AddCharacter(characterProgress);

        OnNewCharacterIsOpened?.Invoke(characterID);
    }
}