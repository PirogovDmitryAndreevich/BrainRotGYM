using UnityEngine;
using UnityEngine.UI;

public class GymController : ShowerAbstractClass
{
    [Header("Buttons")]
    [SerializeField] private Button _horizontalBar;
    [SerializeField] private Button _bench;
    [SerializeField] private Button _balks;
    [SerializeField] private Button _foots;

    protected override void OnDestroy()
    {
        base.OnDestroy();

        // Очистка listeners кнопок
        _horizontalBar.onClick.RemoveAllListeners();
        _bench.onClick.RemoveAllListeners();
        _balks.onClick.RemoveAllListeners();
        _foots.onClick.RemoveAllListeners();
    }

    public override void Initialize()
    {
        base.Initialize();

        _identificate = Identificate.GYM;

        _horizontalBar.onClick.AddListener(() => ShowerManager.Instance.onShowOnScreen?.Invoke(Identificate.HorizontalBar));
        _bench.onClick.AddListener(() => ShowerManager.Instance.onShowOnScreen?.Invoke(Identificate.Bench));
        _balks.onClick.AddListener(() => ShowerManager.Instance.onShowOnScreen?.Invoke(Identificate.Balks));
        _foots.onClick.AddListener(() => ShowerManager.Instance.onShowOnScreen?.Invoke(Identificate.Foots));

    }

    protected override void ShowOnScreen(Identificate identificate)
    {
        base.ShowOnScreen(identificate);
    }

    protected override void Hide()
    {
        base.Hide();
    }

    protected override void Show()
    {
        base.Show();
    }

    protected override void ShowingIsCompleted()
    {
        base.ShowingIsCompleted();
    }
}
