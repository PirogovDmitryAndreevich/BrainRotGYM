using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FlyingUpScoreEffect : MonoBehaviour
{
    public static FlyingUpScoreEffect Instance;

    [SerializeField] private Canvas _canvas;
    [SerializeField] private Transform _parentObject;
    [SerializeField] private float flyDistance = 100f;
    [SerializeField] private float flyDuration = 1.5f;


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

    public void CreateFlyingText(GameObject prefab, Vector3 screenPosition)
    {
        if (prefab == null) return;
        if (_parentObject == null) return;

        GameObject flyingText = Instantiate(prefab, _parentObject);

        RectTransform rectTransform = flyingText.GetComponent<RectTransform>();
        if (rectTransform == null) return;

        // Устанавливаем начальные параметры RectTransform
        rectTransform.localScale = Vector3.one;
        rectTransform.rotation = Quaternion.identity;

        Vector2 localPoint;
        RectTransform canvasRect = _canvas.GetComponent<RectTransform>();

        // Input.mousePosition уже в экранных координатах, используем напрямую
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect,
            screenPosition,
            _canvas.worldCamera,
            out localPoint))
        {
            rectTransform.anchoredPosition = localPoint;
            Debug.Log($"Screen: {screenPosition} -> Local: {localPoint}");
        }
        else
        {
            // Fallback
            rectTransform.anchoredPosition = Vector2.zero;
            Debug.LogWarning("Failed to convert screen point to local rectangle.");
        }

        // Запускаем анимацию
        StartCoroutine(AnimateFlyingText(rectTransform, flyDistance, flyDuration));
    }

    private IEnumerator AnimateFlyingText(RectTransform rectTransform, float flyDistance, float flyDuration)
    {
        Vector2 startPosition = rectTransform.anchoredPosition;
        Vector2 targetPosition = startPosition + Vector2.up * flyDistance;

        // Получаем компоненты для анимации
        CanvasGroup canvasGroup = rectTransform.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = rectTransform.gameObject.AddComponent<CanvasGroup>();
        }

        float elapsed = 0f;

        while (elapsed < flyDuration)
        {
            elapsed += Time.deltaTime;
            float progress = elapsed / flyDuration;

            // Движение вверх
            rectTransform.anchoredPosition = Vector2.Lerp(startPosition, targetPosition, progress);

            // Затухание прозрачности
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, progress);

            yield return null;
        }

        // Уничтожаем объект после анимации
        Destroy(rectTransform.gameObject);
    }

    private Vector2 GetWorldPointToLocalPoint(Vector3 point)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
       _canvas.transform as RectTransform,
       RectTransformUtility.WorldToScreenPoint(Camera.main, point),
       Camera.main,
       out Vector2 output);

        return output;
    }
}

