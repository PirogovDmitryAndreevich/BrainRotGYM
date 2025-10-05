using UnityEngine;

[RequireComponent(typeof(ButtonBalks))]
public class Balks : ShowerAbstractClass
{
    private ButtonBalks _button;

    public override void Initialize()
    {
        base.Initialize();
        _identifierEnum = Identificate.Balks;
        _button = GetComponent<ButtonBalks>();
        _button.Initialize(_identifierEnum);
    }
}
