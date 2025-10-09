using System;
using UnityEngine;

[DefaultExecutionOrder(-100)]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public Action OnScenesIsReady;
    private int _maxScenes;

    public Action OnShowMoverIsReady;

    private void Awake()
    {
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

        _maxScenes = Enum.GetValues(typeof(Identificate)).Length;

        WaitingLoad.Instance.WaitAndExecute
        (
            () => ShowManager.Instance != null && ShowManager.Instance.Scenes.Count == _maxScenes,
            () => OnScenesIsReady?.Invoke()            
        );
    }
}
