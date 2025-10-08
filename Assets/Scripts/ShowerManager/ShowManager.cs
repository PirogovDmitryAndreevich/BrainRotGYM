using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof (SceneSwitcher))]
public class ShowManager : MonoBehaviour
{
    public static ShowManager Instance;

    [Header("Scene settings")]
    [SerializeField] private Button _backButton;

    [Header("Character setting")]
    [SerializeField] private CharacterManager _characterManager;

    public List<ShowerAbstractClass> Scenes;
    public Action<Identificate> OnShowOnScreen;

    private SceneSwitcher _sceneSwitcher;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            _sceneSwitcher = GetComponent<SceneSwitcher>();
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
                Debug.Log($"ShowManager started initialize scenes");
                GameManager.Instance.OnScenesIsReady += InitializeScenes;
                OnShowOnScreen += Show;
            }
        );

        _backButton.gameObject.SetActive(false);
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

        WaitingLoad.Instance.WaitAndExecute
        (
            () => ShowerMover.Instance != null,
            () => OnShowOnScreen?.Invoke(Identificate.GYM)
        );        
    }    

    private void Show(Identificate identifier)
    {
        foreach (var scene in Scenes)
        {
            if (scene.Identifier == identifier)            
                SwitchScene(scene);            
        }
    }

    private void SwitchScene(ShowerAbstractClass targetScene)
    {
        _characterManager.Showing(targetScene.Identifier);
        _sceneSwitcher.Show(targetScene);
        _backButton.gameObject.SetActive(targetScene.Identifier != Identificate.GYM);
    }
}