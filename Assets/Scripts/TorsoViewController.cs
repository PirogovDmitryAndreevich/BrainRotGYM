using UnityEngine;

public class TorsoViewController : MonoBehaviour
{
    [Header("Press Parts")]
    [SerializeField] private BodyPart[] _pressParts;

    public void UpdateSprites(int lvl)
    {
        // ������������ ������� � ������ (������� 1 = ������ 0, ������� 5 = ������ 4)
        int spriteIndex = lvl - 1;

        foreach (var part in _pressParts)
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
