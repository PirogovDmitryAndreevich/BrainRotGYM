using System.Collections.Generic;
using UnityEngine;

public class CharacterProgressManager : MonoBehaviour 
{
    private Dictionary<CharactersEnum, CharacterProgressData> _openedCharactersDic = new();

    public Dictionary<CharactersEnum, CharacterProgressData> OpenedCharacters => _openedCharactersDic;

    private void Awake()
    {
        WaitingLoad.Instance.WaitAndExecute
            (
                () => Progress.Instance != null && Progress.Instance.PlayerInfo != null,
                () => LoadProgress()
            );
    }

    public void LoadProgress()
    {
        _openedCharactersDic.Clear();

        if (Progress.Instance.PlayerInfo.OpenedCharacters == null)
        {
            Progress.Instance.PlayerInfo.OpenedCharacters = new List<CharacterProgressData>();
        }

        foreach (var characterData in Progress.Instance.PlayerInfo.OpenedCharacters)
        {
            if (characterData != null && !_openedCharactersDic.ContainsKey(characterData.CharacterID))
                _openedCharactersDic[characterData.CharacterID] = characterData;
        }
    } 
}