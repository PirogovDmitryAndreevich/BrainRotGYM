using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerInfoPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;

    private int _score;

    public Action OnScoreChanged;

    private void Awake()
    {
        GameManager.Instance.OnAllSystemsReady += Initialize;
        OnScoreChanged += UpdateScoreUI;
    }

    private void OnDestroy()
    {
        GameManager.Instance.OnAllSystemsReady -= Initialize;
        OnScoreChanged -= UpdateScoreUI;
    }

    public int Score
    {
        get => _score;
        set
        {
            if (_score != value)
            {
                _score = value;
                Progress.Instance.PlayerInfo.Score = _score;
                Progress.Instance.Save();
                OnScoreChanged?.Invoke();
            }
        }
    }

    public void UpdateScoreUI()
    {
        _text.text = _score.ToString();
    }

    private void Initialize()
    {
        _score = Progress.Instance.PlayerInfo.Score;
        UpdateScoreUI();
    }
}
