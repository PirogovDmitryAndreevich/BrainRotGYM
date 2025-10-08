using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MoveToPosition))]
public class ShowerMover : MonoBehaviour
{
    public static ShowerMover Instance;

    [Header("Настройки анимации")]
    [SerializeField] private float _moveHeight = 2f;    
    [SerializeField] private float _moveDuration = 1f;
    [SerializeField] private Transform _target;

    private MoveToPosition _moveToPosition;
    private Dictionary<Transform, Coroutine> _activeCoroutines = new Dictionary<Transform, Coroutine>();

    public Action OnHidingIsCompleted;
    public Action OnShowIsCompleted;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            _moveToPosition= GetComponent<MoveToPosition>();
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public void MoveDown(Vector3 originPosition, Transform targetTransform)
    {
        StopMovement(targetTransform);

        Coroutine coroutine = StartCoroutine(MoveToTargetRoutine(originPosition, targetTransform));
        _activeCoroutines[targetTransform] = coroutine;
    }

    public void MoveUp(Vector3 originPosition, Transform targetTransform)
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
        yield return _moveToPosition.Move(targetTransform, position + Vector3.up * _moveHeight, _moveDuration);

        yield return _moveToPosition.Move(targetTransform, _target.position, _moveDuration);

        OnHidingIsCompleted?.Invoke();

        if (_activeCoroutines.ContainsKey(targetTransform))        
            _activeCoroutines.Remove(targetTransform);        
    }    

    private IEnumerator UpDownMovementRoutine(Vector3 position, Transform targetTransform)
    {
        yield return _moveToPosition.Move(targetTransform, position + Vector3.up * _moveHeight, _moveDuration);

        yield return _moveToPosition.Move(targetTransform, position, _moveDuration);

        OnShowIsCompleted?.Invoke();

        if (_activeCoroutines.ContainsKey(targetTransform))
            _activeCoroutines.Remove(targetTransform);
    }
}
