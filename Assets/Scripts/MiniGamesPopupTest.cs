using UnityEngine;
using UnityEngine.UI;

public class MiniGamesPopupTest : MonoBehaviour
{
    [SerializeField] private PrizePopup _popup;
    [SerializeField] private Button _button;
    [SerializeField] private int _score;

    private void Awake()
    {
        _button.onClick.AddListener(ShowPopup);
    }

    private void ShowPopup()
    {
        _popup.ShowPopup(_score);
    }
}
