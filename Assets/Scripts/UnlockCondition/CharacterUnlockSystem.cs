using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterUnlockSystem : MonoBehaviour
{
    private CharacterDatabase _database;
    private CharactersDataManager _charactersDataManager;
    private PlayerInfo _playerInfo;

    public static event Action<CharactersEnum> OnCharacterEligibilityChanged;

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
        _database = CharacterDatabase.Instance;
        _charactersDataManager = CharactersDataManager.Instance;
        _playerInfo = Progress.Instance.PlayerInfo;

        CheckAllUnlocks();
    }

    public void NotifyStatsChanged()
    {
        CheckAllUnlocks();
    }

    public void CheckAllUnlocks()
    {
        if (_database == null || _playerInfo == null) return;

        foreach (var character in _database.AllCharactersDictionary)
        {
            bool isOpened = OpenedCharactersManager.Instance.IsCharacterOpened(character.Key);
            var characterData = character.Value;

            bool allMet = true;
            if (characterData.unlockConditions != null && characterData.unlockConditions.Count > 0)
            {
                foreach (var cond in characterData.unlockConditions)
                {
                    if (!cond.IsSatisfied(_playerInfo))
                    {
                        allMet = false;
                        break;
                    }
                }
            }
            else
            {
                allMet = false;
            }

            if (isOpened)
                allMet = false;

            OnCharacterEligibilityChanged?.Invoke(character.Key);
        }
    }
}
