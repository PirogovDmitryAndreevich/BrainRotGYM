using System;
using UnityEngine;
using YG;

public class Leaderboard : MonoBehaviour
{
    private const string LBName = "ScoreLB";

    [SerializeField] private LeaderboardYG _leaderboard;

    private PlayerInfo playerInfo;

    private void Awake()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnAllSystemsReady += Initialize;

            if(GameManager.Instance.IsAllSystemsReady)
                Initialize();
        }
    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.OnAllSystemsReady -= Initialize;

        if (Progress.Instance.PlayerInfo != null)
            Progress.Instance.PlayerInfo.OnScoreChanged -= SetNewValueLB;
    }

    private void Initialize()
    {
        Progress.Instance.PlayerInfo.OnScoreChanged += SetNewValueLB;
        playerInfo = Progress.Instance.PlayerInfo;
                
        SetNewValueLB();
    }

    private void SetNewValueLB()
    {
        _leaderboard.SetLeaderboard(playerInfo.Score);
        _leaderboard.UpdateLB();
    }
}
