using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class MiniGamesButtons : MonoBehaviour, IMiniGamesButton
{
    [SerializeField] private MiniGamesType type;

    private Button _button;

    public MiniGamesType Type => type;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(OnClick);
    }

    public void Select()
    {
        SoundEffects.Instance.PlaySelectTournament();
        Debug.Log($"Выбрана мини игра: {type}");
    }

    public void Deselect()
    {
        Debug.Log($"Выход из мини игры: {type}");
    }

    private void OnClick()
    {
        MiniGamesManager.Instance.SelectMiniGame(type, this);
    }
}
