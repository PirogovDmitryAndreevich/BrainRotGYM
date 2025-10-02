using UnityEngine;
using YG;

[System.Serializable]
public class PlayerInfo
{
    public int Balk = 0;
    public int Bench  = 0;
    public int HorizontalBars = 0;
    public int Foots = 0;

    public int LvlBalk = 1;
    public int LvlBench  = 1;
    public int LvlHorizontalBars = 1;
    public int LvlFoots = 1;
}

public class Progress : MonoBehaviour
{
    public PlayerInfo PlayerInfo;

    public static Progress Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            transform.parent = null;
            DontDestroyOnLoad(gameObject);
            Instance = this;
            LoadPlayerInfo();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Save()
    {
        if (PlayerInfo != null)
        {
            YG2.saves.PlayerInfo = PlayerInfo;
            YG2.SaveProgress();
        }
    }

    public void LoadPlayerInfo()
    {
        PlayerInfo = YG2.saves.PlayerInfo ?? new PlayerInfo();        
    }
}
