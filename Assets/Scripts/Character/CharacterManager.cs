using System;
using UnityEngine;

[RequireComponent(typeof(CharacterViewController), typeof(CharacterAnimation))]
public class CharacterManager : MonoBehaviour
{
    [SerializeField] private float _upward;

    private CharacterProgressData _currentProgressCharacter;
    private CharacterData _currentViewCharacter;

    private CharacterViewController _viewController;
    private CharacterAnimation _animation;
    private Vector2 _gymPosition;
    private Vector2 _trainingPosition;

    private Identificate _currentSceneIdentifier;

    private void Awake()
    {        
        _viewController = GetComponent<CharacterViewController>();
        _animation = GetComponent<CharacterAnimation>();
        _gymPosition = transform.localPosition;
        _trainingPosition = Vector2.zero;

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

    private void OnDestroy()
    {
       
    }

    public void ShowingOnScene(Identificate identifier)
    {
        Debug.Log($"CharacterManager ShowinOnScene: {identifier}");
        _currentSceneIdentifier = identifier;
    }

    public void AddScore()
    {
        Vector2 fixedScreenPosition = new Vector2(Screen.width / 2, Screen.height / 2);

        FlyingUpScoreEffect.Instance.CreateClickUIEffect(fixedScreenPosition + Vector2.up* _upward, 1); /////////////////////////
        StatsManager.Instance.OnAddStat?.Invoke(_currentSceneIdentifier, 1); /////////////////////////////
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
        if (_currentSceneIdentifier == Identificate.GYM)
            transform.localPosition = _gymPosition;
        else
            transform.localPosition = _trainingPosition;
    }
}
