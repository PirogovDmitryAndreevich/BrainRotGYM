using System;
using UnityEngine;

public enum TrainingArea
{
    Hide,
    Show
}

public class TrainingAreaController : MonoBehaviour
{
    public static TrainingAreaController Instance;

    public Action OnAnimationIsComplete;

    private Animator _animator;

    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);

        Instance = this;
        _animator = GetComponent<Animator>();
    }

    public void SetTrainingArea(TrainingArea trigger)
    {
        _animator.SetTrigger(trigger.ToString());
    }

    public void AnimationInform()
    {
        OnAnimationIsComplete?.Invoke();
    }
}
