using UnityEngine;

[RequireComponent (typeof(ButtonFoots))]
public class Foots : SceneElementBase
{
    private ButtonFoots _button;

    public override void Initialize()
    {
        base.Initialize();
        _identifierEnum = Identificate.Foots;
        _button = GetComponent<ButtonFoots>();
        _button.Initialize(_identifierEnum);
    }
}
