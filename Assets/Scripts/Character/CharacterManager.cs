using UnityEngine;

[RequireComponent (typeof(CharacterViewController))]
public class CharacterManager : MonoBehaviour
{
    private int _lvl;
    private const int MaxLvl = 5;
    private CharacterViewController _viewController;

    private void Awake()
    {
        _viewController = GetComponent<CharacterViewController>();
        _lvl = 1;
    }

    private void Start()
    {
        UpdateView();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            _lvl = 1;
            UpdateView();
        }
    }

    public void TestUpdate()
    {
        _lvl++;
        _lvl = Mathf.Clamp(_lvl, 1, MaxLvl);
        UpdateView();
    }

    private void UpdateView()
    {
        _viewController.UpdateLvlView(_lvl);
    }
}
