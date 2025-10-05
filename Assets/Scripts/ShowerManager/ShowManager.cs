using System;
using System.Collections.Generic;
using UnityEngine;

public class ShowManager : MonoBehaviour
{
    public static ShowManager Instance;

    [SerializeField] private Transform _trainingArea;

    public List<ShowerAbstractClass> Scenes;
    public Action<Identificate> OnShowOnScreen;

    private ShowerAbstractClass _currentScene;

    private void Awake()
    {
        Debug.Log("ShowerManager: Instance created");

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        WaitingLoad.Instance.WaitAndExecute(
            () => GameManager.Instance != null,
            () =>
            {
                GameManager.Instance.OnScenesIsReady += InitializeScenes;
                OnShowOnScreen += Show;
            }
        );
    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.OnScenesIsReady -= InitializeScenes;

        OnShowOnScreen -= Show;
    }

    private void InitializeScenes()
    {
        foreach (var scene in Scenes)
            scene.Initialize();

        WaitingLoad.Instance.WaitAndExecute(
            () => ShowerMover.Instance != null,
            () => OnShowOnScreen?.Invoke(Identificate.GYM));

        // Безопасное выключение trainingArea
        if (_trainingArea != null)
            _trainingArea.gameObject.SetActive(false);
    }

    private void Show(Identificate identifier)
    {
        if (ShowerMover.Instance == null) return;

        ShowerAbstractClass targetScene = null;

        foreach (var scene in Scenes)
        {
            if (scene != null && scene.Identifier == identifier)
            {
                targetScene = scene;
                break;
            }
        }

        if (targetScene == null) return;
        if (_currentScene != null && _currentScene.Identifier == identifier) return;

        var sceneToHide = _currentScene;
        _currentScene = targetScene;

        if (sceneToHide != null)
        {
            ShowerMover.Instance.Hide(sceneToHide.OriginalPosition, sceneToHide.transform);

            if (_trainingArea != null && _trainingArea.gameObject.activeSelf)
                ShowerMover.Instance.Hide(_trainingArea.position, _trainingArea.transform);

            ShowerMover.Instance.OnHidingIsCompleted += () => OnSceneHidden(sceneToHide, targetScene);
        }
        else
        {
            ShowScene(targetScene);
        }
    }

    private void OnSceneHidden(ShowerAbstractClass hiddenScene, ShowerAbstractClass targetScene)
    {
        ShowerMover.Instance.OnHidingIsCompleted -= () => OnSceneHidden(hiddenScene, targetScene);

        hiddenScene.gameObject.SetActive(false);

        if (_trainingArea != null)
            _trainingArea.gameObject.SetActive(false);

        ShowScene(targetScene);
    }

    private void ShowScene(ShowerAbstractClass scene)
    {
        _currentScene = scene;
        scene.gameObject.SetActive(true);

        if (scene.Identifier != Identificate.GYM && _trainingArea != null)
        {
            _trainingArea.gameObject.SetActive(true);
            MoveTrainingAreaToScene(scene);
            ShowerMover.Instance.Show(_trainingArea.position, _trainingArea.transform);
        }

        ShowerMover.Instance.Show(scene.OriginalPosition, scene.transform);
        ShowerMover.Instance.OnShowIsCompleted += OnShowCompleted;
    }

    private void OnShowCompleted()
    {
        ShowerMover.Instance.OnShowIsCompleted -= OnShowCompleted;
        Debug.Log($"Show completed for: {_currentScene.Identifier}");
    }

    private void MoveTrainingAreaToScene(ShowerAbstractClass scene)
    {
        if (_trainingArea != null && scene.transform != null)
        {
            _trainingArea.position = scene.transform.position;
            _trainingArea.rotation = scene.transform.rotation;
        }
    }
}