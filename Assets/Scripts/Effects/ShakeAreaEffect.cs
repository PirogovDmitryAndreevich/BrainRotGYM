using System.Collections;
using UnityEngine;

public class ShakeAreaEffect : MonoBehaviour
{
    [SerializeField] private Transform _areaForShake;

    private float _shakeIntensity = 2f; // Интенсивность тряски
    private float _shakeDuration = 0.3f; // Длительность тряски
    private float _shakeSpeed = 50f;

    private Vector2 _originalRotation;
    private Coroutine _shakeCoroutine;

    private void Awake()
    {
        _originalRotation = _areaForShake.localEulerAngles;
    }

    public void Shake()
    {
        if (_areaForShake == null)
        {
            Debug.LogWarning("ShakeAreaEffect: Area for shake is not assigned!");
            return;
        }

        // Останавливаем предыдущую тряску если она активна
        if (_shakeCoroutine != null)
        {
            StopCoroutine(_shakeCoroutine);
        }

        // Запускаем новую корутину тряски
        _shakeCoroutine = StartCoroutine(ShakeAnimation());
    }

    private IEnumerator ShakeAnimation()
    {
        float elapsed = 0f;

        while (elapsed < _shakeDuration)
        {
            elapsed += Time.deltaTime;
            float progress = elapsed / _shakeDuration;

            // Вычисляем текущую интенсивность (уменьшаем к концу)
            float currentIntensity = _shakeIntensity * (1f - progress);

            // Генерируем тряску используя синус и косинус для плавности
            float shakeZ = Mathf.Sin(Time.time * _shakeSpeed) * currentIntensity;
            float additionalShake = Mathf.Cos(Time.time * _shakeSpeed * 1.7f) * currentIntensity * 0.3f;

            // Комбинируем тряски для более естественного эффекта
            float totalShake = shakeZ + additionalShake;

            // Применяем тряску только по оси Z
            Vector3 newRotation = _originalRotation;
            newRotation.z += totalShake;
            _areaForShake.localEulerAngles = newRotation;

            yield return null;
        }

        // Возвращаем объект в исходное положение
        _areaForShake.localEulerAngles = _originalRotation;

        // Сбрасываем ссылку на корутину
        _shakeCoroutine = null;
    }
}
