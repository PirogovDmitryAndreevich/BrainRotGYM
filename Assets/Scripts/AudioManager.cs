using System;
using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio setting")]
    [SerializeField] private float _duration = 0.5f;
    [SerializeField] private float _volume = 0.3f;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource _firstSource;
    [SerializeField] private AudioSource _secondSource;

    [Header("Audio clips")]
    [SerializeField] private AudioClip _bgTrainingClip;
    [SerializeField] private AudioClip _bgGYMClip;
    [SerializeField] private AudioClip _bgArmWrestlingClip;
    [SerializeField] private AudioClip _bgTeamDeadliftClip;

    [Header("Editor")]
    [SerializeField] private AudioSource _currentSource;
    private Coroutine _crossFadeRoutine;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        PlayGYMMusic();
    }

    public void PlayTrainingMusic() => PlayMusic(_bgTrainingClip);
    public void PlayGYMMusic() => PlayMusic(_bgGYMClip);
    public void PlayArmWrestlingMusic() => PlayMusic(_bgArmWrestlingClip);
    public void PlayTeamDeadliftMusic() => PlayMusic(_bgTeamDeadliftClip);

    public void MuteMusic()
    {
        if (_currentSource != null)
        {
            if (_crossFadeRoutine != null)
                StopCoroutine(_crossFadeRoutine);

            _crossFadeRoutine = StartCoroutine(Mute());
        }
    }

    // ---------------------- MAIN LOGIC ------------------------

    public void PlayMusic(AudioClip clip)
    {
        if (_currentSource != null && _currentSource.clip == clip)
            return;

        AudioSource newSource = (_currentSource == _firstSource) ? _secondSource : _firstSource;

        PlayMusic(newSource, clip);
    }

    private void PlayMusic(AudioSource newSource, AudioClip clip)
    {
        if (_crossFadeRoutine != null)
            StopCoroutine(_crossFadeRoutine);

        _crossFadeRoutine = StartCoroutine(CrossFade(_currentSource, newSource, clip, _duration));
        _currentSource = newSource;
    }

    private IEnumerator CrossFade(AudioSource oldSource, AudioSource newSource, AudioClip newClip, float duration)
    {
        float time = 0f;

        float oldStartVolume = oldSource != null ? _volume : 0f;
        float newTargetVolume = _volume;

        newSource.clip = newClip;
        newSource.volume = 0f;
        newSource.Play();

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;

            if (oldSource != null)
                oldSource.volume = Mathf.Lerp(oldStartVolume, 0f, t);

            newSource.volume = Mathf.Lerp(0f, newTargetVolume, t);

            yield return null;
        }

        if (oldSource != null)
        {
            oldSource.volume = 0f;
            oldSource.Stop();
        }

        newSource.volume = newTargetVolume;
    }

    private IEnumerator Mute()
    {
        float startVolume = _currentSource.volume;
        float time = 0f;

        while (time < _duration)
        {
            time += Time.deltaTime;
            float t = time / _duration;

            _currentSource.volume = Mathf.Lerp(startVolume, 0f, t);

            yield return null;
        }

        _currentSource.Stop();
        _currentSource = null;
    }
}
