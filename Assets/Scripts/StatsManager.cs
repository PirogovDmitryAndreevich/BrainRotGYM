using System;
using UnityEngine;

public class StatsManager : MonoBehaviour
{
    public static StatsManager Instance;

    private int _balks = 0;
    private int _bench = 0;
    private int _horizontalBars = 0;
    private int _foots = 0;

    public Action<Stats, int> AddStat;

    private void Awake()
    {
        Debug.Log("StatsPlayer: Instance created");

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

        AddStat += AddingStat;
    }

    private void OnDestroy()
    {
        AddStat -= AddingStat;
    }

    private void AddingStat(Stats stat, int value)
    {
        switch (stat)
        {
            case Stats.Balks:
                _balks += value;
                break;
            case Stats.Bench:
                _bench += value;
                break;
            case Stats.HorizontalBar:
                _horizontalBars += value;
                break;
            case Stats.Foots:
                _foots += value;
                break;
            default:
                break;
        }
    }
}
