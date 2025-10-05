using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowerMover : MonoBehaviour
{
    public static ShowerMover Instance;

    [Header("Настройки анимации")]
    [SerializeField] private float _moveHeight = 2f;    // Высота подъема
    [SerializeField] private float _moveDuration = 1f;
    [SerializeField] private Transform _target;

    private Dictionary<Transform, Coroutine> _activeCoroutines = new Dictionary<Transform, Coroutine>();

    public Action OnHidingIsCompleted;
    public Action OnShowIsCompleted;

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
            return;
        }
    }

    public void Hide(Vector3 originPosition, Transform targetTransform)
    {
        StopMovement(targetTransform);

        Coroutine coroutine = StartCoroutine(MoveToTargetRoutine(originPosition, targetTransform));
        _activeCoroutines[targetTransform] = coroutine;
    }

    public void Show(Vector3 originPosition, Transform targetTransform)
    {
        StopMovement(targetTransform);

        Coroutine coroutine = StartCoroutine(UpDownMovementRoutine(originPosition, targetTransform));
        _activeCoroutines[targetTransform] = coroutine;
    }

    private void StopMovement(Transform targetTransform)
    {
        if (_activeCoroutines.ContainsKey(targetTransform))
        {
            if (_activeCoroutines[targetTransform] != null)
            {
                StopCoroutine(_activeCoroutines[targetTransform]);
            }
            _activeCoroutines.Remove(targetTransform);
        }
    }

    private IEnumerator MoveToTargetRoutine(Vector3 position, Transform targetTransform)
    {
        yield return MoveToPosition(position + Vector3.up * _moveHeight, _moveDuration, targetTransform);

        yield return MoveToPosition(_target.position, _moveDuration, targetTransform);

        OnHidingIsCompleted?.Invoke();

        if (_activeCoroutines.ContainsKey(targetTransform))        
            _activeCoroutines.Remove(targetTransform);        
    }    

    private IEnumerator UpDownMovementRoutine(Vector3 position, Transform targetTransform)
    {
        yield return MoveToPosition(position + Vector3.up * _moveHeight, _moveDuration, targetTransform);

        yield return MoveToPosition(position, _moveDuration, targetTransform);

        OnShowIsCompleted?.Invoke();

        if (_activeCoroutines.ContainsKey(targetTransform))
            _activeCoroutines.Remove(targetTransform);
    }

    private IEnumerator MoveToPosition(Vector3 targetPosition, float duration, Transform targetTransform)
    {
        Vector3 startPosition = targetTransform.position;
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            float progress = time / duration;

            float smoothProgress = Mathf.SmoothStep(0f, 1f, progress);

            targetTransform.position = Vector3.Lerp(startPosition, targetPosition, smoothProgress);
            yield return null;
        }

        targetTransform.position = targetPosition;
    }

}
