using UnityEngine;

[RequireComponent (typeof(ButtonBench))]
public class Bench : ShowerAbstractClass
{
    private ButtonBench _button;

    public override void Initialize()
    {
        base.Initialize();
        _identifierEnum = Identificate.Bench;
        _button = GetComponent<ButtonBench>();
        _button.Initialize(_identifierEnum);
    }
}
