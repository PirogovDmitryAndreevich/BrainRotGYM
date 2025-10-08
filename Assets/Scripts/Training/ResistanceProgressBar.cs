using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ResistanceProgressBar : MonoBehaviour
{
    private const float MinFillValue = 0f;
    private const float BaseFillPerClick = 0.15f;
    private const float BaseMaxFill = 1f;
    private const float BaseResistance = 0.1f;

    [SerializeField] private Image _progressFill;

    private Coroutine _resistanceCoroutine;
    private Coroutine _fillCoroutine;

    private bool _isCompleted = false;
    private bool _isFilling = false;

    private float _resistance;
    private float _fillAmountPerClick;

    public Action OnProgressBarIsCompleted;

    private void OnEnable()
    {
        if (_resistanceCoroutine == null && _progressFill != null)
        {
            _resistanceCoroutine = StartCoroutine(ResistanceRoutine());
        }

        OnProgressBarIsCompleted += FillAmountIsCompleted;
    }

    private void OnDisable()
    {
        if (_resistanceCoroutine != null)
        {
            StopCoroutine(_resistanceCoroutine);
            _resistanceCoroutine = null;
        }

        if (_fillCoroutine != null)
        {
            StopCoroutine(_fillCoroutine);
            _fillCoroutine = null;
            _isFilling = false;
        }

        OnProgressBarIsCompleted -= FillAmountIsCompleted;
    }

    public void Initialize(int playerLevel)
    {
        playerLevel = Mathf.Max(1, playerLevel);

        _resistance = BaseResistance + (0.015f * (playerLevel - 1));
        _fillAmountPerClick = Mathf.Max(0.04f, BaseFillPerClick - (0.02f * (playerLevel - 1)));
        _progressFill.fillAmount = MinFillValue;

        if (_resistanceCoroutine == null && gameObject.activeSelf)
        {
            _resistanceCoroutine = StartCoroutine(ResistanceRoutine());
        }
    }

    public void OnButtonClick()
    {
        
        if (_isCompleted || _isFilling || _progressFill == null) return;

        // Останавливаем предыдущую анимацию заполнения если есть
        if (_fillCoroutine != null)
        {
            StopCoroutine(_fillCoroutine);
        }

        // Запускаем плавное заполнение
        _fillCoroutine = StartCoroutine(SmoothFill());
    }

    private IEnumerator SmoothFill()
    {
        _isFilling = true;

        float startFill = _progressFill.fillAmount;
        float targetFill = startFill + _fillAmountPerClick;
        targetFill = Mathf.Clamp(targetFill, MinFillValue, BaseMaxFill);

        float duration = 0.2f; // Длительность анимации заполнения
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float progress = elapsed / duration;

            // Плавная интерполяция с easing
            float easedProgress = EaseOutCubic(progress);
            _progressFill.fillAmount = Mathf.Lerp(startFill, targetFill, easedProgress);

            yield return null;
        }

        // Гарантируем точное значение
        _progressFill.fillAmount = targetFill;
        _isFilling = false;
        _fillCoroutine = null;

        // Проверяем завершение после анимации
        if (_progressFill.fillAmount >= BaseMaxFill && !_isCompleted)
        {
            _isCompleted = true;
            OnProgressBarIsCompleted?.Invoke();
        }
    }

    private float EaseOutCubic(float t)
    {
        return 1f - Mathf.Pow(1f - t, 3f);
    }

    private IEnumerator ResistanceRoutine()
    {
        while (true)
        {
            if (!_isCompleted && !_isFilling) 
            {
                // Постоянное уменьшение из-за сопротивления
                _progressFill.fillAmount -= _resistance * Time.deltaTime;
                _progressFill.fillAmount = Mathf.Clamp(_progressFill.fillAmount, MinFillValue, BaseMaxFill);

                // Проверяем завершение
                if (_progressFill.fillAmount >= BaseMaxFill && !_isCompleted)
                {
                    _isCompleted = true;
                    OnProgressBarIsCompleted?.Invoke();
                }
            }

            yield return null;
        }
    }

    private void FillAmountIsCompleted()
    {
        _progressFill.fillAmount = MinFillValue;
        _isCompleted = false;
    }
}
