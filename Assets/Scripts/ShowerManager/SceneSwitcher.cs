using System;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent (typeof(ShowerMover))]
public class SceneSwitcher : MonoBehaviour
{
    [SerializeField] private Transform _trainingArea;

    public Action OnSceneIsSwitched;

    private ShowerMover _mover;
    private ShowerAbstractClass _targetScene;
    private ShowerAbstractClass _currentScene;
    private Vector2 _trainingAreaOriginPosition;

    private void Awake()
    {
        _mover = GetComponent<ShowerMover>();

        if (_trainingArea != null)
        {
            _trainingArea.gameObject.SetActive(false);
            _trainingAreaOriginPosition = _trainingArea.transform.position;
        }
    }

    public void Show (ShowerAbstractClass targetScene)
    {
        if (targetScene == null) return;

        _targetScene = targetScene;

        if (_currentScene != null)
        {
            HideCurrentScene();
        }
        else
        {
            ShowTargetScene();
        }
    }

    private void HideCurrentScene()
    {
        _mover.MoveDown(_currentScene.OriginalPosition, _currentScene.transform);
        _mover.MoveDown(_trainingAreaOriginPosition, _trainingArea);

        _mover.OnHidingIsCompleted += OnCurrentSceneHidden;
    }

    private void ShowTargetScene()
    {
        _targetScene.gameObject.SetActive(true);
        _trainingArea.gameObject.SetActive(true);

        _mover.MoveUp(_targetScene.OriginalPosition, _targetScene.transform);
        _mover.MoveUp(_trainingAreaOriginPosition, _trainingArea);

        _mover.OnShowIsCompleted += OnTargetSceneShown;
    }

    private void OnCurrentSceneHidden()
    {
        _mover.OnHidingIsCompleted -= OnCurrentSceneHidden;

        _currentScene.gameObject.SetActive(false);
        _trainingArea.gameObject.SetActive(false);

        ShowTargetScene();
    }    

    private void OnTargetSceneShown()
    {
        _mover.OnShowIsCompleted -= OnTargetSceneShown;

        _currentScene = _targetScene;
        _targetScene = null;

        OnSceneIsSwitched?.Invoke();
    }
}
