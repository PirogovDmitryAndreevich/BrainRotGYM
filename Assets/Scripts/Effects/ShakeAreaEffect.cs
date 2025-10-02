using System.Collections;
using UnityEngine;

public class ShakeAreaEffect : MonoBehaviour
{
    [SerializeField] private Transform _areaForShake;

    private float _shakeIntensity = 2f; // ������������� ������
    private float _shakeDuration = 0.3f; // ������������ ������
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

        // ������������� ���������� ������ ���� ��� �������
        if (_shakeCoroutine != null)
        {
            StopCoroutine(_shakeCoroutine);
        }

        // ��������� ����� �������� ������
        _shakeCoroutine = StartCoroutine(ShakeAnimation());
    }

    private IEnumerator ShakeAnimation()
    {
        float elapsed = 0f;

        while (elapsed < _shakeDuration)
        {
            elapsed += Time.deltaTime;
            float progress = elapsed / _shakeDuration;

            // ��������� ������� ������������� (��������� � �����)
            float currentIntensity = _shakeIntensity * (1f - progress);

            // ���������� ������ ��������� ����� � ������� ��� ���������
            float shakeZ = Mathf.Sin(Time.time * _shakeSpeed) * currentIntensity;
            float additionalShake = Mathf.Cos(Time.time * _shakeSpeed * 1.7f) * currentIntensity * 0.3f;

            // ����������� ������ ��� ����� ������������� �������
            float totalShake = shakeZ + additionalShake;

            // ��������� ������ ������ �� ��� Z
            Vector3 newRotation = _originalRotation;
            newRotation.z += totalShake;
            _areaForShake.localEulerAngles = newRotation;

            yield return null;
        }

        // ���������� ������ � �������� ���������
        _areaForShake.localEulerAngles = _originalRotation;

        // ���������� ������ �� ��������
        _shakeCoroutine = null;
    }
}
