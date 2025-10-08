using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    private const string DefaultStateName = "Default";

    public void Play(Identificate identifier)
    {
        _animator.SetTrigger(identifier.ToString());
    }

    public void SwitchDefaultState()
    {
        _animator.Play(DefaultStateName);
    }
}
