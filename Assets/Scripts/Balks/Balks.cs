public class Balks : ShowerAbstractClass
{
    protected override void OnDestroy()
    {
        base.OnDestroy();
    }

    public override void Initialize()
    {
        base.Initialize();
        _identificate = Identificate.Balks;
    }

    protected override void ShowOnScreen(Identificate identificate)
    {
        base.ShowOnScreen(identificate);
    }

    protected override void Show()
    {
        base.Show();
    }

    protected override void ShowingIsCompleted()
    {
        base.ShowingIsCompleted();
    }

    protected override void Hide()
    {
        base.Hide();
    }
}
