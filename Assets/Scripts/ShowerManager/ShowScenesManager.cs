using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(SceneSwitcher))]
public class ShowScenesManager : MonoBehaviour
{
    public static ShowScenesManager Instance;

    [SerializeField] public Transform FinalTarget;
    [SerializeField] public float MoveHeight = 2f;
    [SerializeField] public float MoveDuration = 1f;
    [SerializeField] public Button BackButton; 

    public SceneElementBase CurrentScene;
    public List<SceneElementBase> Scenes; 

    private SceneSwitcher _sceneSwitcher;

    public Action<Identificate> SwitchScene;
    public Action<Identificate> OnShowOnScreen;

    private Dictionary<Identificate, SceneElementBase> _scenesDict = new Dictionary<Identificate, SceneElementBase>();

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        _sceneSwitcher = GetComponent<SceneSwitcher>();
        _sceneSwitcher.OnSwitchIsComplete += OnSwitchComplete;

        WaitGameManager();
        OnShowOnScreen += ShowScene;
    }

    private void OnDestroy()
    {
        OnShowOnScreen -= ShowScene;
        GameManager.Instance.OnAllSystemsReady -= InitializeScenes;
        _sceneSwitcher.OnSwitchIsComplete -= OnSwitchComplete;
    }

    private void WaitGameManager()
    {
        WaitingLoad.Instance.WaitAndExecute(
                () => GameManager.Instance != null,
                () =>
                {
                    if (GameManager.Instance.IsAllSystemsReady == true)
                        InitializeScenes();
                    else
                        GameManager.Instance.OnAllSystemsReady += InitializeScenes;
                }
            );
    }

    private void InitializeScenes()
    {
        foreach (var scene in Scenes)
        {
            scene.Initialize();
            if (!_scenesDict.ContainsKey(scene.Identifier))
            {
                _scenesDict[scene.Identifier] = scene;
            }
            scene.gameObject.SetActive(false);
        }

        _sceneSwitcher.ShowScene( _scenesDict[Identificate.GYM], CurrentScene);
    }

    private void ShowScene(Identificate identifier)
    {
        _sceneSwitcher.ShowScene(_scenesDict[identifier], CurrentScene);
    }

    private void OnSwitchComplete(Identificate identifier)
    {
        CurrentScene = _scenesDict[identifier];
        SwitchScene?.Invoke(identifier);
    }

}