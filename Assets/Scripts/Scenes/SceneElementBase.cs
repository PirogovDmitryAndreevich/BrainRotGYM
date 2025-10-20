using UnityEngine;

public abstract class SceneElementBase : MonoBehaviour, ISceneElement
{
    [SerializeField] protected Identificate _identifierEnum;

    protected Vector3 _originalPosition;
    protected bool _isInitialized = false;

    public Identificate Identifier => _identifierEnum;
    public Vector3 OriginalPosition => _originalPosition;
    public bool IsInitialized => _isInitialized;


    private void Awake()
    {
        if (ShowScenesManager.Instance != null)
        {
            ShowScenesManager.Instance.Scenes.Add(this);
        }
        else
        {
            WaitingLoad.Instance.WaitAndExecute
                (
                    () => ShowScenesManager.Instance != null,
                    () => ShowScenesManager.Instance.Scenes.Add(this)
                );
        }

    }


    public virtual void Initialize()
    {
        if (_isInitialized) return;

        _originalPosition = transform.position;
        _isInitialized = true;
    }
}