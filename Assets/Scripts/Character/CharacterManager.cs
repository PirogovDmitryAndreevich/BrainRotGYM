using UnityEngine;

[RequireComponent(typeof(CharacterViewController), typeof(CharacterAnimation))]
public class CharacterManager : MonoBehaviour
{
    private const int PassiveDefaultAddScore = 1;

    [SerializeField] private float _upward;

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

        WaitingLoad.Instance.WaitAndExecute
            (
                () => FlyingUpScoreEffect.Instance != null,
                () => _effectInstance = FlyingUpScoreEffect.Instance
             );

        WaitingLoad.Instance.WaitAndExecute
            (
                () => StatsManager.Instance != null,
                () => statsManagerInstance = StatsManager.Instance
            );

        WaitingLoad.Instance.WaitAndExecute
            (
                () => ShowManager.Instance != null,
                () => ShowManager.Instance.OnCharacterInitialize += Initialize
            );
    }

    private void OnEnable()
    {
        _animation.Play(_currentSceneIdentifier);
        SetPosition();
    }

    public void ShowingOnScene(Identificate identifier)
    {
        _currentSceneIdentifier = identifier;
    }

    public void AddScore()
    {
        Vector2 fixedScreenPosition = new Vector2(Screen.width / 2, Screen.height / 2);

        _effectInstance.CreateClickUIEffect(effectPosition, PassiveDefaultAddScore);
        statsManagerInstance.OnAddStat?.Invoke(_currentSceneIdentifier, PassiveDefaultAddScore);
    }

    private void Initialize()
    {
        _currentProgressCharacter = Progress.Instance.PlayerInfo.CurrentCharacter;
        _currentViewCharacter = CharactersDataManager.Instance.CurrentCharacterView;
        UpdateView();        
    }

    private void UpdateView()
    {
        _viewController.UpdateCharacterView(_currentViewCharacter);
    }

    private void UpdateLvlView()
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
}
