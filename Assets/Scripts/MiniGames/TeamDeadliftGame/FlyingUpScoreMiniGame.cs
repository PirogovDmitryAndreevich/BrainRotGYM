using System.Collections;
using UnityEngine;

public class FlyingUpScoreMiniGame : MonoBehaviour
{
    private RectTransform _rectTransform;
    private float _duration;
    private float _flyDistance;
    private CanvasGroup _canvasGroup;

    public void StartFlyingUp(Vector2 instantiatePosition, GameObject prefab, float duration, float flyDistance)
    {
        prefab.transform.position = instantiatePosition;

        if (prefab.TryGetComponent<CanvasGroup>(out CanvasGroup canvasGroup))
        {
            _canvasGroup = canvasGroup;
        }
        else
        {
            prefab.AddComponent<CanvasGroup>();
            _canvasGroup = prefab.GetComponent<CanvasGroup>();
        }

        _flyDistance = flyDistance;
        _rectTransform = prefab.GetComponent<RectTransform>();
        _duration = duration;

        StartCoroutine(Flying());
    }

    private IEnumerator Flying()
    {
        Vector2 startPosition = _rectTransform.anchoredPosition;
        Vector2 targetPosition = startPosition + Vector2.up * _flyDistance;
        
        // Добавляем случайное небольшое смещение по X для разнообразия
        float randomOffset = Random.Range(-30f, 30f);
        Vector2 curvedPath = new Vector2(randomOffset, 0f);

        // Начальный scale для эффекта появления
        Vector3 startScale = Vector3.zero;
        Vector3 normalScale = Vector3.one;
        Vector3 endScale = Vector3.one * 0.8f;

        float elapsed = 0f;

        while (elapsed < _duration)
        {
            elapsed += Time.deltaTime;
            float progress = elapsed / _duration;

            // Кривые для разных аспектов анимации
            float moveProgress = EaseOutQuad(progress);
            float scaleProgress = EaseOutBack(Mathf.Clamp01(progress * 2f)); // Быстрая анимация scale
            float fadeProgress = EaseInQuad(progress);
            float curveProgress = EaseOutSine(progress);

            // Движение вверх с легкой кривой
            Vector2 currentPosition = Vector2.Lerp(startPosition, targetPosition, moveProgress);
            currentPosition += curvedPath * curveProgress;
            _rectTransform.anchoredPosition = currentPosition;

            // Анимация scale - быстро появляется, затем немного уменьшается
            if (progress < 0.5f)
            {
                _rectTransform.localScale = Vector3.Lerp(startScale, normalScale, scaleProgress);
            }
            else
            {
                _rectTransform.localScale = Vector3.Lerp(normalScale, endScale, (progress - 0.5f) * 2f);
            }

            // Затухание прозрачности (начинается позже)
            float alphaFadeStart = 0.3f;
            if (progress > alphaFadeStart)
            {
                float fadeAmount = (progress - alphaFadeStart) / (1f - alphaFadeStart);
                _canvasGroup.alpha = Mathf.Lerp(1f, 0f, EaseInQuad(fadeAmount));
            }
            else
            {
                _canvasGroup.alpha = 1f;
            }

            // Легкое вращение для динамичности
            _rectTransform.rotation = Quaternion.Euler(0f, 0f, Mathf.Sin(progress * Mathf.PI * 4f) * 3f);

            yield return null;
        }

        // Уничтожаем объект после анимации
        Destroy(_rectTransform.gameObject);
    }

    private float EaseOutQuad(float t)
    {
        return 1f - (1f - t) * (1f - t);
    }

    private float EaseInQuad(float t)
    {
        return t * t;
    }

    private float EaseOutBack(float t)
    {
        float c1 = 1.70158f;
        float c3 = c1 + 1f;
        return 1f + c3 * Mathf.Pow(t - 1f, 3f) + c1 * Mathf.Pow(t - 1f, 2f);
    }

    private float EaseOutSine(float t)
    {
        return Mathf.Sin((t * Mathf.PI) / 2f);
    }
}
