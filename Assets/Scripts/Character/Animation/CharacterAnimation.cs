using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{
    private const string DefaultStateName = "Default";
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
    }

    public void Play(Identificate identifier)
    {
        _animator.SetTrigger(identifier.ToString());
    }

    public void SwitchDefaultState()
    {
        _animator.Play(DefaultStateName);
    }
}
