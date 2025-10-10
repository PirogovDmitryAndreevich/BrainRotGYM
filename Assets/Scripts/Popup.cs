using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Popup : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private CanvasGroup _popupCanvasGroup;
    [SerializeField] private RectTransform _popupPanel;
    [SerializeField] private Button _closeButton;
    [SerializeField] private Button _openButton;

    [Header("Animation Settings")]
    [SerializeField] private float _fadeDuration = 0.3f;
    [SerializeField] private float _scaleDuration = 0.2f;

    private void Awake()
    {
        _openButton.onClick.AddListener(ShowPopup);
        _closeButton.onClick.AddListener(HidePopup);
        // Скрыть попап сразу
        _popupCanvasGroup.alpha = 0f;
        _popupCanvasGroup.blocksRaycasts = false;
    }

    private void OnDestroy()
    {
        if (_closeButton != null)
            _closeButton.onClick.RemoveAllListeners();

        if (_openButton != null)
            _openButton.onClick.RemoveAllListeners();
    }

    private void ShowPopup()
    {
        StopAllCoroutines();
        StartCoroutine(ShowPopupCoroutine());
    }

    private void HidePopup()
    {
        StopAllCoroutines();
        StartCoroutine(HidePopupCoroutine());
    }

    private IEnumerator ShowPopupCoroutine()
    {
        _popupCanvasGroup.blocksRaycasts = true;

        // Анимация появления
        float elapsedTime = 0f;
        Vector3 startScale = Vector3.zero;
        Vector3 endScale = Vector3.one;

        while (elapsedTime < _fadeDuration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            float progress = elapsedTime / _fadeDuration;

            // Фейд
            _popupCanvasGroup.alpha = Mathf.Lerp(0f, 1f, progress);

            // Скейл
            _popupPanel.localScale = Vector3.Lerp(startScale, endScale, progress / _scaleDuration);

            yield return null;
        }

        _popupCanvasGroup.alpha = 1f;
        _popupPanel.localScale = endScale;
    }

    private IEnumerator HidePopupCoroutine()
    {
        _popupCanvasGroup.blocksRaycasts = false;

        float elapsedTime = 0f;
        float startAlpha = _popupCanvasGroup.alpha;

        while (elapsedTime < _fadeDuration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            float progress = elapsedTime / _fadeDuration;
            _popupCanvasGroup.alpha = Mathf.Lerp(startAlpha, 0f, progress);
            yield return null;
        }

        _popupCanvasGroup.alpha = 0f;
    }
}
