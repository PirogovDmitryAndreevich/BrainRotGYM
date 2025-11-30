using UnityEngine;
using UnityEngine.UI;

public class AddStatsScoreTest : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private int score;
    [SerializeField] private Identificate type;

    private void Awake()
    {
        _button.onClick.AddListener(AddScore);
    }

    private void AddScore()
    {
        StatsManager.Instance.OnAddStat?.Invoke(type, score);
    }
}
