using UnityEngine;

public class DecorationViewController : ViewControllerAbstract, ISetSpriteCharacter
{
    public void SetSprite(Sprite sprite)
    {
        foreach (var part in _parts)
            part.renderer.sprite = sprite;
    }
}

