using UnityEngine;

public class CharacterLevelManager : MonoBehaviour
{
    private static CharacterLevelManager instance;

    private CharacterProgressData _currentCharacter;
    private UpdateManager _updateManager;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        WaitForDependencies();
    }

    private void OnDestroy()
    {
        if (CharactersDataManager.Instance != null)
            CharactersDataManager.Instance.OnSelectedCharacter -= SetCurrentCharacter;

        if (_updateManager != null)
            _updateManager.OnUpdatingLevel -= UpdateLevel;
    }

    private void WaitForDependencies()
    {
        WaitingLoad.Instance.WaitAndExecute(
            () => CharactersDataManager.Instance != null && Progress.Instance?.PlayerInfo?.CurrentCharacter != null,
            () =>
            {
                CharactersDataManager.Instance.OnSelectedCharacter += SetCurrentCharacter;
                SetCurrentCharacter();
            }
        );

        WaitingLoad.Instance.WaitAndExecute(
            () => UpdateManager.Instance != null,
            () =>
            {
                _updateManager = UpdateManager.Instance;
                _updateManager.OnUpdatingLevel += UpdateLevel;
            }
        );
    }

    private void SetCurrentCharacter()
    {
        _currentCharacter = Progress.Instance.PlayerInfo.CurrentCharacter;

        if (_updateManager != null)
        {
            _updateManager.OnCheckUpdateLevel?.Invoke();
        }
    }

    private void UpdateLevel()
    {
        _currentCharacter.UpdateLevel();
        _updateManager.OnLevelUpdated?.Invoke();
        _updateManager.OnCheckUpdateLevel?.Invoke();
        Progress.Instance.Save();
    }
}