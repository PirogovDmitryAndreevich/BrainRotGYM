using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(SceneSwitcher))]
public class ShowManager : MonoBehaviour
{
    public static ShowManager Instance;

    [Header("Scene settings")]
    [SerializeField] private Button _backButton;

    [Header("Character setting")]
    [SerializeField] private CharacterManager _characterManager;

    public List<ShowerAbstractClass> Scenes;
    public Action<Identificate> OnShowOnScreen;
    public Action OnCharacterInitialize;

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

        WaitingLoad.Instance.WaitAndExecute
        (
            () => GameManager.Instance != null,
            () =>
            {
                GameManager.Instance.OnScenesIsReady += InitializeScenes;
                OnShowOnScreen += Show;
            }
        );
       
        WaitingLoad.Instance.WaitAndExecute
        (
            () => Progress.Instance != null && CharactersMenu.Instance != null,
            () => CharactersMenu.Instance.OnSelectedCharacter += InitializeCharacter
        );

        _backButton.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnScenesIsReady -= InitializeScenes;
        }

        if (CharactersMenu.Instance != null)
        {
            CharactersMenu.Instance.OnSelectedCharacter -= InitializeCharacter;
        }

        OnShowOnScreen -= Show;
    }

    private void InitializeScenes()
    {
        Debug.Log($"ShowManager start initialize scenes");

        foreach (var scene in Scenes)
            scene.Initialize();

        WaitingLoad.Instance.WaitAndExecute
        (
            () => ShowerMover.Instance != null && Progress.Instance.PlayerInfo.CurrentCharacter != null,
            () => OnShowOnScreen?.Invoke(Identificate.GYM)
        );
    }

    private void InitializeCharacter()
    {
        WaitingLoad.Instance.WaitAndExecute
            (
                () => StatsManager.Instance != null,
                () => OnCharacterInitialize?.Invoke()
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
        Debug.Log($"ShowManager SwitchScene: {targetScene.Identifier}, targetScene: {targetScene.transform.name}");
        _characterManager.ShowingOnScene(targetScene.Identifier);
        _sceneSwitcher.Show(targetScene);
        _backButton.gameObject.SetActive(targetScene.Identifier != Identificate.GYM);
    }
}