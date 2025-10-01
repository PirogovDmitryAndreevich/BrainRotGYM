using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
[RequireComponent(typeof(Button))]
public abstract class ShowerAbstractClass : MonoBehaviour
{
    protected Identificate _identificate;
    protected bool _isInitialized;
    protected Vector3 _originalPosition;
    protected Button _button;

    protected virtual void OnDestroy()
    {
        // Отписка от событий
        if (ShowerManager.Instance != null)
            ShowerManager.Instance.onShowOnScreen -= ShowOnScreen;

        if (ShowerMover.Instance != null)
        {
            ShowerMover.Instance.HidingIsCompleted -= Show;
            ShowerMover.Instance.HidingIsCompleted -= Hide;
            ShowerMover.Instance.ShowIsCompleted -= ShowingIsCompleted;
        }

        _button.onClick.RemoveAllListeners();
    }

    public virtual void Initialize() 
    {
        if (_isInitialized) return;

        _button = GetComponent<Button>();
        _button.onClick.AddListener(OnClickButton);

        Debug.Log($"{transform.name} is initialized");
        _originalPosition = transform.position;

        if (ShowerManager.Instance != null)
            ShowerManager.Instance.onShowOnScreen += ShowOnScreen;
        else
            return;

        _isInitialized = true;
        gameObject.SetActive(false);
    }

    protected virtual void ShowOnScreen(Identificate identificate)
    {
        if (identificate != _identificate)
        {
            ShowerMover.Instance.HidingIsCompleted += Hide;
           ShowerMover.Instance.Hide(_originalPosition, transform);
        }
        else
        {
            ShowerMover.Instance.HidingIsCompleted += Show;
        }
    }

    protected virtual void Show()
    {
        ShowerMover.Instance.HidingIsCompleted -= Show;

        gameObject.SetActive(true);
        ShowerMover.Instance.ShowIsCompleted += ShowingIsCompleted;
        ShowerMover.Instance.Show(_originalPosition, transform);
    }

    protected virtual void ShowingIsCompleted()
    {
        ShowerMover.Instance.ShowIsCompleted -= ShowingIsCompleted;
    }

    protected virtual void Hide()
    {
        ShowerMover.Instance.HidingIsCompleted -= Hide;
        gameObject.SetActive(false);
    }

    protected virtual void OnClickButton()
    {

    }
}
