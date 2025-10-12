using UnityEngine;

public class ViewControllerAbstract : MonoBehaviour
{
    [Header("Parts")]
    [SerializeField] protected BodyPart[] _parts;

    public void SetColor(Color color)
    {
        foreach (var part in _parts)
        {
            part.renderer.color = color;
        }
    }

    public void SetSprite(Sprite sprite)
    {
        foreach (var part in _parts)
        {
            if (sprite != null)
                part.renderer.sprite = sprite;
            else
                part.renderer.sprite = null;
        }
    }

    public void UpdateSprites(int lvl)
    {
        // ������������ ������� � ������ (������� 1 = ������ 0, ������� 5 = ������ 4)
        int spriteIndex = lvl - 1;

        foreach (var part in _parts)
        {
            // ��������� ��� ������ ����������, renderer ���������� � ������ � �������� �������
            if (part.sprites != null && part.renderer != null && spriteIndex < part.sprites.Length && spriteIndex >= 0)
            {
                part.renderer.sprite = part.sprites[spriteIndex];
            }
            else
            {
                Debug.LogWarning($"�� ������� �������� ������ ��� {part.renderer?.name}. ������: {spriteIndex}, ����� �������: {part.sprites?.Length}");
            }
        }
    }
}
