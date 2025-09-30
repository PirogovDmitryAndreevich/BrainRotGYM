using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GymMover : MonoBehaviour
{
    [Header("Настройки анимации")]
    [SerializeField] private float _moveHeight = 2f;    // Высота подъема
    [SerializeField] private float _moveDuration = 1f;
    [SerializeField] private Transform _target;

    private Vector3 _originalPosition;
    private Coroutine _currentCoroutine;

    private void Awake()
    {
        _originalPosition = transform.position;
    }

    public void MoveAway()
    {
        StartMovement(MoveToTargetRoutine());
    }

    public void Remove()
    {
        gameObject.SetActive(true);
        StartMovement(UpDownMovementRoutine());
    }

    private void StopMovement()
    {
        if (_currentCoroutine != null)
        {
            StopCoroutine(_currentCoroutine);
            _currentCoroutine = null;
        }
    }

    private void StartMovement(IEnumerator routine)
    {
        StopMovement();
        _currentCoroutine = StartCoroutine(routine);
    }

    private IEnumerator MoveToTargetRoutine()
    {
        yield return MoveToPosition(_originalPosition + Vector3.up * _moveHeight, _moveDuration);

        yield return MoveToPosition(_target.position, _moveDuration);

        gameObject.SetActive(false);
    }

    private IEnumerator UpDownMovementRoutine()
    {        

        yield return MoveToPosition(_originalPosition + Vector3.up * _moveHeight, _moveDuration);

        yield return MoveToPosition(_originalPosition, _moveDuration);
    }

    private IEnumerator MoveToPosition(Vector3 targetPosition, float duration)
    {
        Vector3 startPosition = transform.position;
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            float progress = time / duration;

            float smoothProgress = Mathf.SmoothStep(0f, 1f, progress);

            transform.position = Vector3.Lerp(startPosition, targetPosition, smoothProgress);
            yield return null;
        }

        // Гарантируем точное достижение целевой позиции
        transform.position = targetPosition;
    }


}
