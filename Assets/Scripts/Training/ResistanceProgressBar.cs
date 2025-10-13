using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ResistanceProgressBar : MonoBehaviour
{
    private const float MinFillValue = 0f;
    private const float BaseMaxFill = 100f;
    private const float BaseFillPerClick = 10f;

    [SerializeField] private Image _progressFill;

    private Coroutine _resistanceCoroutine;
    private Coroutine _fillCoroutine;

    private bool _isCompleted = false;
    private bool _isFilling = false;

    private float _currentFill = 0f;
    private int _playerLevel = 1;
    private int _completedCycles = 0; // ������� ��� ��������� ����������
    private float _currentResistance;

    public Action OnProgressBarIsCompleted;
    public Action OnProgressBarIsReset;

    private void OnEnable()
    {
        if (_resistanceCoroutine == null && _progressFill != null)
        {
            _resistanceCoroutine = StartCoroutine(ResistanceRoutine());
        }

        OnProgressBarIsCompleted += FillAmountIsCompleted;
        OnProgressBarIsReset += ResetResistance;

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
        OnProgressBarIsReset -= ResetResistance;
    }

    public void Initialize(int playerLevel)
    {
        playerLevel = Mathf.Max(1, playerLevel);

        _currentFill = MinFillValue;
        _completedCycles = 0;

        UpdateResistance();
        UpdateVisualFill();
    }

    public void OnButtonClick()
    {
        
        if (_isCompleted || _isFilling || _progressFill == null) return;

        // ������������� ���������� �������� ���������� ���� ����
        if (_fillCoroutine != null)
        {
            StopCoroutine(_fillCoroutine);
        }

        // ��������� +1 � ����������
        _currentFill += BaseFillPerClick;
        _currentFill = Mathf.Clamp(_currentFill, MinFillValue, BaseMaxFill);

        // ��������� ������� ��������
        _fillCoroutine = StartCoroutine(SmoothFill());

        // ��������� ����������
        if (_currentFill >= BaseMaxFill && !_isCompleted)
        {
            _isCompleted = true;
            OnProgressBarIsCompleted?.Invoke();
        }
    }

    private void UpdateResistance()
    {
        // ������� �������������:
        // ������� ������������� + �� ������� + �� ���������� ����������

        float baseResistance = 3f;
        float levelMultiplier = 4f * _playerLevel; // +0.02 �� ������ �������
        float cyclesMultiplier = 7f * _completedCycles; // +0.015 �� ������ ����������

        _currentResistance = baseResistance + levelMultiplier + cyclesMultiplier;

        Debug.Log($"Resistance updated: Level={_playerLevel}, Cycles={_completedCycles}, Resistance={_currentResistance:F3}");
    }

    private void FillAmountIsCompleted()
    {
        _completedCycles++;
        _currentFill /= 2; // ���������� �� �������� ����� ����������
        _isCompleted = false;

        // ��������� ������������� ����� ������� ����������
        UpdateResistance();

        Debug.Log($"Cycle completed! Total cycles: {_completedCycles}");
    }

    private void ResetResistance()
    {
        _completedCycles = 0;
    }

    private IEnumerator SmoothFill()
    {
        _isFilling = true;

        float startVisualFill = _progressFill.fillAmount;
        float targetVisualFill = _currentFill / BaseMaxFill; // ������������ � 0-1 ��� Image

        float duration = 0.15f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float progress = elapsed / duration;

            float easedProgress = EaseOutCubic(progress);
            _progressFill.fillAmount = Mathf.Lerp(startVisualFill, targetVisualFill, easedProgress);

            yield return null;
        }

        _progressFill.fillAmount = targetVisualFill;
        _isFilling = false;
        _fillCoroutine = null;

        if (_currentFill <= MinFillValue)
            OnProgressBarIsReset?.Invoke();

        // ��������� ���������� ����� ��������
        if (_progressFill.fillAmount >= BaseMaxFill && !_isCompleted)
        {
            _isCompleted = true;
            OnProgressBarIsCompleted?.Invoke();
        }
    }

    private void UpdateVisualFill()
    {
        if (_progressFill != null)
        {
            _progressFill.fillAmount = _currentFill / BaseMaxFill;
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
                // ���������� ���������� ��-�� �������������
                _currentFill -= _currentResistance * Time.deltaTime;
                _currentFill = Mathf.Clamp(_currentFill, MinFillValue, BaseMaxFill);

                UpdateVisualFill();

                if(_currentFill <= MinFillValue)
                    OnProgressBarIsReset?.Invoke();

                // ��������� ����������
                if (_currentFill >= BaseMaxFill && !_isCompleted)
                {
                    _isCompleted = true;
                    OnProgressBarIsCompleted?.Invoke();
                }
            }

            yield return null;
        }
    }
}
