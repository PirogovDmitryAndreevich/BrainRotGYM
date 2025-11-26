using TMPro;
using UnityEngine;

public class ScorePrefabEffect : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreText;

    public void SetScoreValue(int score)
    {
        _scoreText.text = score.ToString();
    }
}
