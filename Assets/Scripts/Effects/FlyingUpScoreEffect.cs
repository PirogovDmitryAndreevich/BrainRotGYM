using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FlyingUpScoreEffect : MonoBehaviour
{
    public static FlyingUpScoreEffect Instance;

    [SerializeField] private Canvas _canvas;
    [SerializeField] private Transform _parentObject;
    [SerializeField] private float _flyDistance = 100f;
    [SerializeField] private float _flyDuration = 1.5f;
    [SerializeField] private float _clickEffectDuration = 0.5f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void CreateClickUIEffect(Vector2 clickMousePos, int score)
    {
        Debug.Log($"CreateClickUIEffect called at {clickMousePos} with score {score}");

        if (_parentObject == null)
        {
            Debug.LogError("_parentObject is null!");
            return;
        }

        if (MyPrefabs.Instance == null)
        {
            Debug.LogError("MyPrefabs.Instance is null!");
            return;
        }

        // Получаем локальную позицию на канвасе
        Vector2 localPosition = GetCanvasLocalPosition(clickMousePos);
        Debug.Log($"Canvas local position: {localPosition}");

        // Создаем эффекты
        CreateFlyingText(localPosition, score);
        CreateClickEffect(localPosition);
    }

    private void CreateFlyingText(Vector2 localPosition, int score)
    {
        if (MyPrefabs.Instance.ScorePrefab == null)
        {
            Debug.LogError("ScorePrefab is null!");
            return;
        }

        GameObject flyingText = Instantiate(MyPrefabs.Instance.ScorePrefab, _parentObject);

        // АКТИВИРУЕМ объект после создания
        flyingText.SetActive(true);

        Debug.Log($"Flying text instantiated: {flyingText != null}");

        // Ищем Text компонент (включая неактивные)
        Text text = flyingText.GetComponentInChildren<Text>(true);
        if (text == null)
        {
            Debug.LogError("Text component not found in children! Available components:");
            Component[] components = flyingText.GetComponentsInChildren<Component>(true);
            foreach (Component comp in components)
            {
                Debug.Log($"Found: {comp.GetType()} in {comp.gameObject.name}");
            }
            Destroy(flyingText);
            return;
        }

        // Активируем родительский объект текста если нужно
        text.gameObject.SetActive(true);
        text.text = score.ToString();
        Debug.Log($"Text set to: {score}");

        RectTransform rectTransform = SetupRectTransform(flyingText, localPosition);
        if (rectTransform == null)
        {
            Debug.LogError("Failed to setup RectTransform!");
            Destroy(flyingText);
            return;
        }

        // Запускаем анимацию
        StartCoroutine(AnimateFlyingText(rectTransform));
    }

    private void CreateClickEffect(Vector2 localPosition)
    {
        if (MyPrefabs.Instance.ClickEffect == null)
        {
            Debug.LogError("ClickEffect is null!");
            return;
        }

        // Создаем эффект клика из префаба
        GameObject clickEffect = Instantiate(MyPrefabs.Instance.ClickEffect, _parentObject);
        clickEffect.SetActive(true); // Активируем

        // Настраиваем RectTransform
        RectTransform rectTransform = SetupRectTransform(clickEffect, localPosition);
        if (rectTransform != null)
        {
            // Запускаем анимацию эффекта клика
            StartCoroutine(AnimateClickEffect(rectTransform));
        }
    }

    private IEnumerator AnimateClickEffect(RectTransform rectTransform)
    {
        float elapsed = 0f;

        while (elapsed < _clickEffectDuration)
        {
            elapsed += Time.deltaTime;
            float progress = elapsed / _clickEffectDuration;

            // Простая анимация затухания
            CanvasGroup canvasGroup = rectTransform.GetComponent<CanvasGroup>();
            if (canvasGroup != null)
            {
                canvasGroup.alpha = Mathf.Lerp(1f, 0f, EaseInQuad(progress));
            }

            // Легкое увеличение scale
            rectTransform.localScale = Vector3.one * Mathf.Lerp(1f, 1.3f, EaseOutQuad(progress));

            yield return null;
        }

        // Уничтожаем эффект после анимации
        Destroy(rectTransform.gameObject);
    }

    private IEnumerator AnimateFlyingText(RectTransform rectTransform)
    {
        Debug.Log("Starting flying text animation");

        Vector2 startPosition = rectTransform.anchoredPosition;
        Vector2 targetPosition = startPosition + Vector2.up * _flyDistance;

        // Получаем или добавляем компоненты для анимации
        CanvasGroup canvasGroup = rectTransform.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = rectTransform.gameObject.AddComponent<CanvasGroup>();
        }

        // Добавляем случайное небольшое смещение по X для разнообразия
        float randomOffset = Random.Range(-30f, 30f);
        Vector2 curvedPath = new Vector2(randomOffset, 0f);

        // Начальный scale для эффекта появления
        Vector3 startScale = Vector3.zero;
        Vector3 normalScale = Vector3.one;
        Vector3 endScale = Vector3.one * 0.8f;

        float elapsed = 0f;

        while (elapsed < _flyDuration)
        {
            elapsed += Time.deltaTime;
            float progress = elapsed / _flyDuration;

            // Кривые для разных аспектов анимации
            float moveProgress = EaseOutQuad(progress);
            float scaleProgress = EaseOutBack(Mathf.Clamp01(progress * 2f)); // Быстрая анимация scale
            float fadeProgress = EaseInQuad(progress);
            float curveProgress = EaseOutSine(progress);

            // Движение вверх с легкой кривой
            Vector2 currentPosition = Vector2.Lerp(startPosition, targetPosition, moveProgress);
            currentPosition += curvedPath * curveProgress;
            rectTransform.anchoredPosition = currentPosition;

            // Анимация scale - быстро появляется, затем немного уменьшается
            if (progress < 0.5f)
            {
                rectTransform.localScale = Vector3.Lerp(startScale, normalScale, scaleProgress);
            }
            else
            {
                rectTransform.localScale = Vector3.Lerp(normalScale, endScale, (progress - 0.5f) * 2f);
            }

            // Затухание прозрачности (начинается позже)
            float alphaFadeStart = 0.3f;
            if (progress > alphaFadeStart)
            {
                float fadeAmount = (progress - alphaFadeStart) / (1f - alphaFadeStart);
                canvasGroup.alpha = Mathf.Lerp(1f, 0f, EaseInQuad(fadeAmount));
            }
            else
            {
                canvasGroup.alpha = 1f;
            }

            // Легкое вращение для динамичности
            rectTransform.rotation = Quaternion.Euler(0f, 0f, Mathf.Sin(progress * Mathf.PI * 4f) * 3f);

            yield return null;
        }

        Debug.Log("Flying text animation completed");

        // Уничтожаем объект после анимации
        Destroy(rectTransform.gameObject);
    }

    private Vector2 GetCanvasLocalPosition(Vector3 screenPosition)
    {
        Vector2 localPoint;
        RectTransform canvasRect = _canvas.GetComponent<RectTransform>();

        if (_canvas == null)
        {
            Debug.LogError("Canvas is null!");
            return Vector2.zero;
        }

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect,
            screenPosition,
            _canvas.worldCamera,
            out localPoint))
        {
            return localPoint;
        }
        else
        {
            Debug.LogWarning("Failed to convert screen point to local rectangle.");
            return Vector2.zero;
        }
    }

    private RectTransform SetupRectTransform(GameObject uiObject, Vector2 localPosition)
    {
        RectTransform rectTransform = uiObject.GetComponent<RectTransform>();
        if (rectTransform == null)
        {
            Debug.LogError("RectTransform not found on object!");
            return null;
        }

        // Устанавливаем базовые параметры RectTransform
        rectTransform.anchoredPosition = localPosition;
        rectTransform.localScale = Vector3.one;
        rectTransform.rotation = Quaternion.identity;
        rectTransform.pivot = new Vector2(0.5f, 0.5f); // Центр как точка pivot
        rectTransform.anchorMin = new Vector2(0.5f, 0.5f); // Якоря по центру
        rectTransform.anchorMax = new Vector2(0.5f, 0.5f); // Якоря по центру

        return rectTransform;
    }

    // Функции easing для плавных анимаций
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

    private float EaseInOutCubic(float t)
    {
        return t < 0.5f ? 4f * t * t * t : 1f - Mathf.Pow(-2f * t + 2f, 3f) / 2f;
    }
}