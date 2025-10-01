using System;
using UnityEngine;

public class StatsPlayer : MonoBehaviour
{
    public static StatsPlayer Instance;

    private int _balks = 0;
    private int _bench = 0;
    private int _horizontalBars = 0;
    private int _foots = 0;    

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
    }    
}
