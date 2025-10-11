using UnityEngine;

[RequireComponent (typeof(Animator))]
public class CharacterAnimation : MonoBehaviour
{
    private const string DefaultStateName = "Default";
    [SerializeField] private Animator _animator;
/*
    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }*/

    public void Play(Identificate identifier)
    {
        _animator.SetTrigger(identifier.ToString());
    }

    public void SwitchDefaultState()
    {
        _animator.Play(DefaultStateName);
    }
}
