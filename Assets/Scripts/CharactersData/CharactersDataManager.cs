using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(CharacterDatabase), typeof(CharacterProgressManager))]
public class CharactersDataManager : MonoBehaviour
{
    public static CharactersDataManager Instance;

    [Header("Character Database")]
    [SerializeField] private CharacterData[] _characterDataArray;

    [HideInInspector] public CharacterData CurrentCharacterView;
    public CharacterProgressData CurrentCharacterProgress => Progress.Instance.PlayerInfo.CurrentCharacter;

    public Action<CharactersEnum> OnSelectCharacter;
    public Action<CharactersEnum> OnOpenNewCharacter;
    public Action OnSelectedCharacter;

    private CharacterDatabase _database;
    private CharacterProgressManager _progressManager;    

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeManagers();
            OnOpenNewCharacter += OpenNewCharacter;
            OnSelectCharacter += SelectCharacter;
        }
        else
        {
            Destroy(gameObject);
        }

        WaitingLoad.Instance.WaitAndExecute(
            () => Progress.Instance != null,
            () => InitializeCharacters()
        );
    }

    private void OnDestroy()
    {
        OnOpenNewCharacter -= OpenNewCharacter;
        OnSelectCharacter -= SelectCharacter;
    }

    private void InitializeManagers()
    {
        _database = GetComponent<CharacterDatabase>();
        _database.CreateDictionary(_characterDataArray);

        _progressManager =GetComponent<CharacterProgressManager>();
    }

    private void InitializeCharacters()
    {
        _progressManager.LoadProgress();

        if (_progressManager.OpenedCharacters.Count == 0)
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
                var characterID = currentCharacter?.CharacterID ?? _progressManager.OpenedCharacters.Keys.First();
                SelectCharacter(characterID);
            }
        }

    }

    public void SelectCharacter(CharactersEnum characterID)
    {
        if (!_progressManager.OpenedCharacters.ContainsKey(characterID))
        {
            Debug.LogWarning($"Character {characterID} is not opened!");
            return;
        }

        var characterProgress = _progressManager.OpenedCharacters[characterID];
        CurrentCharacterView = _database.AllCharactersDictionary[characterID];
        Progress.Instance.PlayerInfo.CurrentCharacter = characterProgress;
        Progress.Instance.Save();

        OnSelectedCharacter?.Invoke();
    }

    public void OpenNewCharacter(CharactersEnum characterID)
    {
        if (_progressManager.OpenedCharacters.ContainsKey(characterID))
        {
            Debug.LogWarning($"Character {characterID} is already opened!");
            return;
        }

        if (!_database.AllCharactersDictionary.ContainsKey(characterID))
        {
            Debug.LogError($"Character {characterID} not found in database!");
            return;
        }

        var characterProgress = new CharacterProgressData(characterID);

        if (Progress.Instance.PlayerInfo.OpenedCharacters == null)
            Progress.Instance.PlayerInfo.OpenedCharacters = new List<CharacterProgressData>();

        Progress.Instance.PlayerInfo.OpenedCharacters.Add(characterProgress);

        Progress.Instance.Save();
        _progressManager.LoadProgress();
        OnOpenNewCharacter?.Invoke(characterID);
    }    
}