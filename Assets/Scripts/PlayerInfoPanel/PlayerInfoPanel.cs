using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerInfoPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;

    public Action OnScoreChanged;

    private void Awake()
    {
        GameManager.Instance.OnAllSystemsReady += Initialize;
    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.OnAllSystemsReady -= Initialize;

        Progress.Instance.PlayerInfo.OnScoreChanged -= UpdateScoreUI;
    }

    private void Initialize()
    {
        if (Progress.Instance?.PlayerInfo != null)
        {
            Progress.Instance.PlayerInfo.OnScoreChanged += UpdateScoreUI;
            UpdateScoreUI();
        }
        else
        {
            Debug.LogError($"[PlayerInfoPanel] PlayerInfo is null");
        }
    }

    private void UpdateScoreUI()
    {
        _text.text = Progress.Instance.PlayerInfo.Score.ToString();
    }
}
