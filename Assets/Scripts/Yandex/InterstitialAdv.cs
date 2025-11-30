using UnityEngine;
using YG;

public class InterstitialAdv : MonoBehaviour
{
    public static InterstitialAdv Instance;

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

    public void ShowAdv()
    {
        YG2.InterstitialAdvShow();
    }

    private void OnEnable()
    {
        YG2.onOpenInterAdv += OpenInterstitial;
        YG2.onCloseInterAdv += CloseInterstitial;
        YG2.onErrorInterAdv += ErrorInterstitial;
    }

    private void OnDisable()
    {
        YG2.onOpenInterAdv -= OpenInterstitial;
        YG2.onCloseInterAdv -= CloseInterstitial;
        YG2.onErrorInterAdv -= ErrorInterstitial;
    }

    private void OpenInterstitial()
    {
        
        Debug.Log("Interstitial ad open");
    }

    private void CloseInterstitial()
    {
        Debug.Log("Interstitial ad closed");
        
    }

    private void ErrorInterstitial()
    {
        Debug.LogError($"Interstitial ad error");
    }
}
