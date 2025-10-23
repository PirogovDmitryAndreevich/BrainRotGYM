using System;
using UnityEngine;

[DefaultExecutionOrder(-100)]
[RequireComponent(typeof (InitializationSystem))]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public event Action OnAllSystemsReady;
    public bool IsAllSystemsReady = false;

    private IInitializationSystem _initializationSystem;

    private void Awake()
    {        
        if (Instance != null)
        {
            Destroy(gameObject);
            return; 
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        _initializationSystem = GetComponent<InitializationSystem>();

        InitializeSystems();  
    }

    private void OnDestroy()
    {
        _initializationSystem.OnAllConditionsMet -= OnAllConditionsMet;
    }

    private void InitializeSystems()
    {
        int sceneCount = Enum.GetValues(typeof(Identificate)).Length;
        _initializationSystem.RegisterCondition(new ProgressCondition());
        _initializationSystem.RegisterCondition(new ShowManagerCondition(sceneCount));
        _initializationSystem.RegisterCondition(new TrainingAreaCondition());
        _initializationSystem.RegisterCondition(new StatsManagerCondition()); 
        _initializationSystem.RegisterCondition(new OpenedCharacterManagerCondition()); 
        _initializationSystem.RegisterCondition(new CharacterDatabaseCondition()); 
        _initializationSystem.RegisterCondition(new CharacterDataManagerCondition());
        _initializationSystem.RegisterCondition(new UpdateManagerCondition());

        _initializationSystem.OnAllConditionsMet += OnAllConditionsMet;

        _initializationSystem.CheckAllConditions();
    }

    private void OnAllConditionsMet()
    {
        IsAllSystemsReady = true;
        Debug.Log("GameManager: All systems are ready!");
        OnAllSystemsReady?.Invoke();  
    }
}