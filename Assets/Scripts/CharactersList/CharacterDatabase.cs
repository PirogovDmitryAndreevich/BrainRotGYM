using System.Collections.Generic;
using UnityEngine;

public class CharacterDatabase : MonoBehaviour
{
    private Dictionary<CharactersEnum, CharacterData> _characterDataDict = new();

    public Dictionary<CharactersEnum, CharacterData> AllCharactersDictionary => _characterDataDict;

    public void CreateDictionary(CharacterData[] characterDataArray)
    {
        _characterDataDict.Clear();

        foreach (var characterData in characterDataArray)        
            if (characterData != null && !_characterDataDict.ContainsKey(characterData.CharacterID))            
                _characterDataDict[characterData.CharacterID] = characterData;        
    }
}