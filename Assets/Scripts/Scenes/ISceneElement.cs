using UnityEngine;

public interface ISceneElement
{
    Identificate Identifier { get; }
    Vector3 OriginalPosition { get; }
    bool IsInitialized { get; }

    void Initialize();
}