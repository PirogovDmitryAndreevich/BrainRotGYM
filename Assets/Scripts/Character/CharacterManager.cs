using System;
using UnityEngine;

[RequireComponent(typeof(CharacterViewController), typeof(CharacterAnimation))]
public class CharacterManager : MonoBehaviour
{
    private CharacterType _currentCharacter;    

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

    private void Initialize()
    {
        _currentCharacter = Progress.Instance.PlayerInfo.CurrentCharacter;
        UpdateView();
    }

    private void UpdateView()
    {
        _viewController.UpdateCharacterView(_currentCharacter);
    }

    private void UpdateLvlView()
    {
        _viewController.UpdateLvlView(_currentCharacter);
    }

    private void SetPosition()
    {
        if (_currentSceneIdentifier == Identificate.GYM)
            transform.localPosition = _gymPosition;
        else
            transform.localPosition = _trainingPosition;
    }
}
