using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;

[RequireComponent(typeof(CharacterViewController), typeof(CharacterAnimation))]
public class CharacterManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;

    private int _lvl;
    private const int MaxLvl = 5;
    private const int MinLvl = 1;
    private CharacterViewController _viewController;
    private CharacterAnimation _animation;
    private Vector2 _gymPosition;
    private Vector2 _trainingPosition;

    private Identificate _currentIdentifier;

    private void Awake()
    {
        _lvl = MinLvl;
        _viewController = GetComponent<CharacterViewController>();
        _animation = GetComponent<CharacterAnimation>();
        _gymPosition = transform.localPosition;
        _trainingPosition = Vector2.zero;
    }

    private void OnEnable()
    {
        _animation.Play(_currentIdentifier);
        SetPosition();
    }

    private void OnDisable()
    {
        
    }

    public void Showing(Identificate identifier)
    {
        _currentIdentifier = identifier;
    }

    public void SetPosition()
    {
        if (_currentIdentifier == Identificate.GYM)
            transform.localPosition = _gymPosition;
        else
            transform.localPosition = _trainingPosition;
    }
}
