using System;
using System.Collections.Generic;
using UnityEngine;

public class OpenedCharactersManager : MonoBehaviour
{
    public static OpenedCharactersManager Instance;

    public bool IsLoadIsComplete;

    private Dictionary<CharactersEnum, CharacterProgressData> _openedCharactersDic = new();
    private bool _isInitialized;

    public IReadOnlyDictionary<CharactersEnum, CharacterProgressData> OpenedCharacters => _openedCharactersDic;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        GameManager.Instance.OnAllSystemsReady += Initialize;
    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.OnAllSystemsReady -= Initialize;
    }

    private void Initialize()
    {
        if (_isInitialized) return;

        LoadProgress();
        _isInitialized = true;
    }

    public bool IsCharacterOpened(CharactersEnum characterId) => _openedCharactersDic.ContainsKey(characterId);
    public CharacterProgressData GetCharacterData(CharactersEnum characterId) => _openedCharactersDic.GetValueOrDefault(characterId);

    public void AddCharacter(CharacterProgressData characterData)
    {
        if (characterData?.CharacterID == null) return;

        var progressList = Progress.Instance.PlayerInfo.OpenedCharacters;
        if (progressList == null)
            Progress.Instance.PlayerInfo.OpenedCharacters = progressList = new List<CharacterProgressData>();

        if (!progressList.Contains(characterData))
        {
            progressList.Add(characterData);
            _openedCharactersDic[characterData.CharacterID] = characterData;
        }

        Progress.Instance.Save();
    }

    private void LoadProgress()
    {
        _openedCharactersDic.Clear();

        var openedCharacters = Progress.Instance.PlayerInfo.OpenedCharacters;
        if (openedCharacters == null)
        {
            Progress.Instance.PlayerInfo.OpenedCharacters = new List<CharacterProgressData>();
            IsLoadIsComplete = true;
            return;
        }

        foreach (var characterData in openedCharacters)
        {
            if (characterData?.CharacterID != null)
                _openedCharactersDic[characterData.CharacterID] = characterData;
        }

        IsLoadIsComplete = true;
    }
}