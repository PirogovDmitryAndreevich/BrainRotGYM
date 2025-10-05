using UnityEngine;

[RequireComponent (typeof(ButtonHorizontalBar))]
public class HorizontalBar : ShowerAbstractClass
{
    private ButtonHorizontalBar _button;

    public override void Initialize()
    {
        base.Initialize();
        _identifierEnum = Identificate.HorizontalBar;
        _button = GetComponent<ButtonHorizontalBar>();
        _button.Initialize(_identifierEnum);
    }
}
