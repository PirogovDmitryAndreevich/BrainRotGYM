using UnityEngine;
using YG;

public class RewardedAdv : MonoBehaviour
{
    public static RewardedAdv Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Важно: сохраняем между сценами
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        YG2.onRewardAdv += GiveReward;
    }

    private void OnDisable()
    {
        YG2.onRewardAdv -= GiveReward;
    }

    public void ShowRewardAdv(string rewardID)
    {
        YG2.RewardedAdvShow(rewardID);
    }

    public void GiveReward(string rewardID)
    {
        
    }
}
