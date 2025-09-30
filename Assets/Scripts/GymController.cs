using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(GymMover))]
public class GymController : MonoBehaviour
{
    [SerializeField] private Button _horizontalBar;
    [SerializeField] private Button _bench;
    [SerializeField] private Button _balks;

    private GymMover _mover;

    private void Awake()
    {
        _mover = GetComponent<GymMover>();

        _horizontalBar.onClick.AddListener(MoveAwayGYM);
        _bench.onClick.AddListener(MoveAwayGYM);
        _balks.onClick.AddListener(MoveAwayGYM);
    }

    private void MoveAwayGYM()
    {
        _mover.MoveAway();
    }

    public void RemoveGYM()
    {
        _mover.Remove();
    }
}
