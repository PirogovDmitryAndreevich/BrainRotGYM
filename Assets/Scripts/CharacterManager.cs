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

    public void TestUpdate()
    {
        _lvl++;
        _lvl = Mathf.Clamp(_lvl, 1, MaxLvl);
        UpdateView();
    }

    public void UpdateView()
    {
        _viewController.UpdateLvlView(_lvl);
    }
}
