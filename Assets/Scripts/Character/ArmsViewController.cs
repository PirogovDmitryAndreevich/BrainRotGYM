using UnityEngine;

public class ArmsViewController : MonoBehaviour
{
    [Header("Arms Parts")]
    [SerializeField] private BodyPart[] _armsParts;

    public void UpdateSprites(int lvl)
    {
        // Конвертируем уровень в индекс (уровень 1 = индекс 0, уровень 5 = индекс 4)
        int spriteIndex = lvl - 1;

        foreach (var part in _armsParts)
        {
            // Проверяем что массив существует, renderer существует и индекс в пределах массива
            if (part.sprites != null && part.renderer != null && spriteIndex < part.sprites.Length && spriteIndex >= 0)
            {
                part.renderer.sprite = part.sprites[spriteIndex];
            }
            else
            {
                Debug.LogWarning($"Не удалось обновить спрайт для {part.renderer?.name}. Индекс: {spriteIndex}, Длина массива: {part.sprites?.Length}");
            }
        }
    }
}
