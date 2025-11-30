using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundEffects : MonoBehaviour
{
    public static SoundEffects Instance;
    public float Volume
    {
        get => _volume;
        set
        {
            if (_volume != value)
            {
                _volume = value;
                Progress.Instance.PlayerInfo.VolumeEffect = _volume;
                Progress.Instance.Save();
            }
        }
    }

    [SerializeField] private AudioClip TrainingButton;
    [SerializeField] private AudioClip ClickTrainingButton;
    [SerializeField] private AudioClip TrainingPBFull;
    [SerializeField] private AudioClip StatsPBFull;
    [SerializeField] private AudioClip OpenPopupSelected;
    [SerializeField] private AudioClip StartTournament;
    [SerializeField] private AudioClip SelectTournament;
    [SerializeField] private AudioClip SelectCharacter;
    [SerializeField] private AudioClip Fanfare;
    [SerializeField] private AudioClip Applause;
    [SerializeField] private AudioClip PositiveAction;
    [SerializeField] private AudioClip NegativeAction;
    [SerializeField] private AudioClip Timer;
    [SerializeField] private AudioClip ClickButtonDeadlift;
    [SerializeField] private AudioClip CreateScore;

    private float _volume = 0.5f;
    private AudioSource _source;

    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);

        Instance = this;
        DontDestroyOnLoad(gameObject);

        _source = GetComponent<AudioSource>();

        GameManager.Instance.OnAllSystemsReady += SetVolume;

        if (GameManager.Instance.IsAllSystemsReady)
            SetVolume();
    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.OnAllSystemsReady -= SetVolume;
    }

    public void PlayTrainingButton() => _source.PlayOneShot(TrainingButton);
    public void PlayClickTrainingButton() => _source.PlayOneShot(ClickTrainingButton);
    public void PlayTrainingPBFull() => _source.PlayOneShot(TrainingPBFull);
    public void PlayStatsPBFull() => _source.PlayOneShot(StatsPBFull);
    public void PlayOpenPopupSelected() => _source.PlayOneShot(OpenPopupSelected);
    public void PlayStartTournament() => _source.PlayOneShot(StartTournament);
    public void PlaySelectTournament() => _source.PlayOneShot(SelectTournament);
    public void PlaySelectCharacter() => _source.PlayOneShot(SelectCharacter);
    public void PlayFanfare() => _source.PlayOneShot(Fanfare);
    public void PlayApplause() => _source.PlayOneShot(Applause);
    public void PlayPositiveAction() => _source.PlayOneShot(PositiveAction);
    public void PlayNegativeAction() => _source.PlayOneShot(NegativeAction);
    public void PlayTimer() => _source.PlayOneShot(Timer);
    public void PlayClickButtonDeadlift() => _source.PlayOneShot(ClickButtonDeadlift);
    public void PlayCreateScore() => _source.PlayOneShot(CreateScore);


    private void SetVolume() => Volume = Progress.Instance.PlayerInfo.VolumeEffect;

}
