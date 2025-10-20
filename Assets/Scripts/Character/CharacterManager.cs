using System;
using UnityEngine;
using UnityEngine.PlayerLoop;

[RequireComponent(typeof(CharacterViewController), typeof(CharacterAnimation))]
public class CharacterManager : MonoBehaviour
{
    private const int PassiveDefaultAddScore = 1;

    public Action OnUpdateBodyView;

    private CharacterProgressData _currentProgressCharacter;
    private CharacterData _currentViewCharacter;

    private CharacterViewController _viewController;
    private CharacterAnimation _animation;
    private FlyingUpScoreEffect _effectInstance;
    private StatsManager statsManagerInstance;
    private Vector2 _gymPosition;
    private Vector2 _trainingPosition;
    private Vector2 effectPosition;
    private Identificate _currentSceneIdentifier;

    private void Awake()
    {
        _viewController = GetComponent<CharacterViewController>();
        _animation = GetComponent<CharacterAnimation>();
        _gymPosition = transform.localPosition;
        _trainingPosition = Vector2.zero;
        effectPosition = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);

        GameManager.Instance.OnAllSystemsReady += Initialize;
        if (GameManager.Instance.IsAllSystemsReady)
            Initialize();
    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null)
        GameManager.Instance.OnAllSystemsReady -= Initialize;

        if (ShowScenesManager.Instance != null)
            ShowScenesManager.Instance.SwitchScene -= SetCurrentScene;

        if (CharactersDataManager.Instance != null)
            CharactersDataManager.Instance.OnSelectedCharacter -= InitializeNewCharacter;

        UpdateManager.Instance.UpdateBodyView -= UpdateBodyView;
    }

    private void Initialize()
    {
        Debug.Log("[CharacterManager] Initialize");

        ShowScenesManager.Instance.SwitchScene += SetCurrentScene;
        CharactersDataManager.Instance.OnSelectedCharacter += InitializeNewCharacter;
        UpdateManager.Instance.UpdateBodyView += UpdateBodyView;

        statsManagerInstance = StatsManager.Instance;

        WaitingLoad.Instance.WaitAndExecute
            ( 
                () => FlyingUpScoreEffect.Instance != null,
                () => _effectInstance = FlyingUpScoreEffect.Instance
            );

        if (CharactersDataManager.Instance.CurrentCharacterView != null
            && _currentViewCharacter == null)
            InitializeNewCharacter();
    }

    public void AddScore()
    {
        Vector2 fixedScreenPosition = new Vector2(Screen.width / 2, Screen.height / 2);

        _effectInstance.CreateClickUIEffect(effectPosition, PassiveDefaultAddScore);
        statsManagerInstance.OnAddStat?.Invoke(_currentSceneIdentifier, PassiveDefaultAddScore);
    }

    private void InitializeNewCharacter()
    {
        _currentProgressCharacter = Progress.Instance.PlayerInfo.CurrentCharacter;
        _currentViewCharacter = CharactersDataManager.Instance.CurrentCharacterView;

        Debug.Log($"[CharacterManager] select new character {_currentViewCharacter.CharacterID}");

        _viewController.UpdateCharacterView(_currentViewCharacter);
        UpdateBodyView();
    }

    private void UpdateBodyView()
    {
        _viewController.UpdateLvlView(_currentProgressCharacter);
    }

    private void SetPosition()
    {
        var transformCache = transform;
        transformCache.localPosition = _currentSceneIdentifier == Identificate.GYM
            ? _gymPosition
            : _trainingPosition;
    }

    private void SetCurrentScene(Identificate identifier)
    {
        _currentSceneIdentifier = identifier;
        _animation.Play(_currentSceneIdentifier);
        SetPosition();
    }
}
