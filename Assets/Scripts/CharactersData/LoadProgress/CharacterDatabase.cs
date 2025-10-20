using System.Collections.Generic;
using UnityEngine;

public class CharacterDatabase : MonoBehaviour
{
    public static CharacterDatabase Instance;

    [SerializeField] private CharacterData[] _characterDataArray;

    private Dictionary<CharactersEnum, CharacterData> _characterDataDict;

    public IReadOnlyDictionary<CharactersEnum, CharacterData> AllCharactersDictionary => _characterDataDict;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        Initialize();
    }

    private void Initialize()
    {
        _characterDataDict = new Dictionary<CharactersEnum, CharacterData>();

        if (_characterDataArray == null) return;

        foreach (var characterData in _characterDataArray)
        {
            if (characterData?.CharacterID != null)
                _characterDataDict[characterData.CharacterID] = characterData;
        }       
    }

    public CharacterData GetCharacterData(CharactersEnum characterId) => _characterDataDict.GetValueOrDefault(characterId);

    public bool TryGetCharacterData(CharactersEnum characterId, out CharacterData characterData) =>
        _characterDataDict.TryGetValue(characterId, out characterData);

    public bool ContainsCharacter(CharactersEnum characterId) =>
        _characterDataDict.ContainsKey(characterId);

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (Application.isPlaying && _characterDataDict != null)
            Initialize();
    }
#endif
}