using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MyPrefabs : MonoBehaviour
{
    public static MyPrefabs Instance;

    [SerializeField] public GameObject ScorePrefab;

    [SerializeField] public GameObject ClickEffect;

    [SerializeField] public GameObject SelectCharacterButton;

    private void Awake()
    {
        // Проверяем, существует ли уже экземпляр
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Опционально, если нужно сохранять между сценами
        }
        else
        {
            // Если уже существует другой экземпляр, уничтожаем этот
            Destroy(gameObject);
        }
    }
}
