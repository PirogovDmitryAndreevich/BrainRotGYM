using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MyPrefabs : MonoBehaviour
{
    public static MyPrefabs Instance;

    [SerializeField] public GameObject ScorePrefab;
    private Text _scoreText;

    [SerializeField] public GameObject ClickEffect;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            _scoreText = ScorePrefab.GetComponentInChildren<Text>();
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
