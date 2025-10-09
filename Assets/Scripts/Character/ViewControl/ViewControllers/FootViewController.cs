using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootViewController : ViewControllerAbstract, ISetSpriteCharacter
{
    public void SetSprite(Sprite sprite)
    {
        foreach (var part in _parts)
            part.renderer.sprite = sprite;
    }
}
