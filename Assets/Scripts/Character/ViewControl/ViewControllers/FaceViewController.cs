using UnityEngine;

public class FaceViewController : ViewControllerAbstract, ISetSpriteCharacter
{
    public void SetSprite(Sprite sprite)
    {
        foreach (var part in _parts)
            part.renderer.sprite = sprite;
    }
}
