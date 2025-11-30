using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectVolumeController : MonoBehaviour
{
    [SerializeField] private float _volume;

    public float Volume
    {
        get => _volume;
        set
        {
            _volume = value;
            SoundEffects.Instance.Volume = value;
        }
    }
}
