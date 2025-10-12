using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Popup : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private CanvasGroup _popupCanvasGroup;
    [SerializeField] private RectTransform _popupPanel;
    [SerializeField] private Button _openButton;
    [SerializeField] private Button[] _closeButtons;

    [Header("Animation Settings")]
    [SerializeField] private float _scaleDuration = 0.2f;
    [SerializeField] private float _fadeDuration = 0.3f;

    private bool _isOpen = false;

    private void Awake()
    {
        _openButton.onClick.AddListener(ShowPopup);

        foreach (Button button in _closeButtons)
            button.onClick.AddListener(HidePopup);

        // Скрыть попап сразу
        _popupCanvasGroup.alpha = 0f;
        _popupCanvasGroup.blocksRaycasts = false;
    }

    private void OnDestroy()
    {
        foreach (Button button in _closeButtons)
        {
            if (button != null)
                button.onClick.RemoveAllListeners();
        }

        if (_openButton != null)
            _openButton.onClick.RemoveAllListeners();
    }

    private void ShowPopup()
    {
        if (_isOpen == true) return;

        _isOpen = true;
        StopAllCoroutines();
        StartCoroutine(ShowPopupCoroutine());
    }

    private void HidePopup()
    {
        if (_isOpen == false) return;

        _isOpen = false;
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
