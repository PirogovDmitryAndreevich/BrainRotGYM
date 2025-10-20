using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonTransitionEnum : MonoBehaviour
{
    [SerializeField] public Identificate _transitionTo;

    private Button _button;

    private void Awake() => _button = GetComponent<Button>();
    private void OnEnable() => _button.onClick.AddListener(() => ShowScenesManager.Instance.OnShowOnScreen?.Invoke(_transitionTo));
    private void OnDisable() => _button.onClick.RemoveAllListeners();
}
