using System;
using UnityEngine;
using UnityEngine.UI;

public class ShowerManager : MonoBehaviour
{
    public static ShowerManager Instance;

    [SerializeField] private Button _button;
    [SerializeField] private ShowerAbstractClass[] _scenes;

    public Action<Identificate> onShowOnScreen;

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
    }

    private void Start()
    {
        foreach (var scene in _scenes)
        { scene.Initialize(); }

        if (_button != null)
            _button.onClick.AddListener(() => onShowOnScreen?.Invoke(Identificate.GYM));
        else
            Debug.LogWarning("ShowerManager: Back button not assigned!");

        onShowOnScreen?.Invoke(Identificate.GYM);
    }

}
