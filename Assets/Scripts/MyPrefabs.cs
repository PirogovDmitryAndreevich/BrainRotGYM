using TMPro;
using UnityEngine;

public class MyPrefabs : MonoBehaviour
{
    public static MyPrefabs Instance;

    [SerializeField] public GameObject ScorePrefab;
    private TextMeshProUGUI _scoreText; 

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            _scoreText = ScorePrefab.GetComponentInChildren<TextMeshProUGUI>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetValueInScorePrefab(int value)
    {
        _scoreText.text = $"+{value}";
    }
}
