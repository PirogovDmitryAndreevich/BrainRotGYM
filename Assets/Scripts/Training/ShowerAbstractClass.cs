using UnityEngine;

public abstract class ShowerAbstractClass : MonoBehaviour
{
    [SerializeField] protected Identificate _identifierEnum;

    protected Vector3 _originalPosition;    
    protected bool _isInitialized = false;

    public Identificate Identifier => _identifierEnum;

    public Vector3 OriginalPosition => _originalPosition;

    private void Awake()
    {
        WaitingLoad.Instance.WaitAndExecute(
            () => ShowManager.Instance != null,
            () => ShowManager.Instance.Scenes.Add(this));
    }

    public virtual void Initialize() 
    {
        Debug.Log($"{transform.name} is initialized");

        if (_isInitialized) return;        

        _originalPosition = transform.position;

        _isInitialized = true;
        gameObject.SetActive(false);
    }    
}
