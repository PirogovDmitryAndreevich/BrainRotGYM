using UnityEngine;

public abstract class ShowerAbstractClass : MonoBehaviour
{
    [SerializeField] protected Identificate _identifierEnum;
    //[SerializeField] protected Transform _trainerTransform;

    protected Vector3 _originalPosition;    
    protected bool _isInitialized;

    public Identificate Identifier => _identifierEnum;

    public Vector3 OriginalPosition => _originalPosition;

   //public Transform TrainerTransform => _trainerTransform;

    private void Awake()
    {
        WaitingLoad.Instance.WaitAndExecute(
            () => ShowManager.Instance != null,
            () => ShowManager.Instance.Scenes.Add(this));
    }

    public virtual void Initialize() 
    {
        if (_isInitialized) return;        

        Debug.Log($"{transform.name} is initialized");
        _originalPosition = transform.position;

        _isInitialized = true;
        gameObject.SetActive(false);
    }    
}
