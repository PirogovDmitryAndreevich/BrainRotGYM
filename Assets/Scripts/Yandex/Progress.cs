using UnityEngine;
using YG;

[System.Serializable]
public class PlayerInfo
{
    public int Balk;
    public int Bench;
    public int HorizontalBars;
    public int Foots;
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
