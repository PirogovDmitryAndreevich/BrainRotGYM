using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ArmBattleGame : MonoBehaviour, IMiniGames
{
    private MiniGamesType _type = MiniGamesType.ArmBattle;
    public MiniGamesType Type  => _type;
    public int MaxCharacters => _maxCharacters;

    private const int LvlCoefficient = 40;
    private const float ResistanceOpponent = 0.1f;

    [Header("Game Settings")]
    [SerializeField] private GameObject _gamePanel;
    [SerializeField] private Button _exitButton;
    [SerializeField] private int _maxCharacters = 1;
    [SerializeField] private float _gameDuration = 10f;
    [SerializeField] private float _timer;
    [SerializeField] private PrizePopup _prize;
    [SerializeField] private int _winScore = 100;
    [SerializeField] private int _loseScore = 0;

    [Header("Balance Bar")]
    [SerializeField] private RectTransform _resultPointer;
    [SerializeField] private float _moveStep = 10f;
    private float _spinAngle = 0f;

    [Header("Spin Settings")]
    [SerializeField] private Slider _slider;
    [SerializeField] private RectTransform _greenZone;
    [SerializeField] private Image _sliderPointer;
    [SerializeField] private float _sliderMoveSpeed = 0.3f;

    [Header("UI Elements")]
    [SerializeField] private Button _pressButton;
    [SerializeField] private TMP_Text _timerText;
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private Image _characterImage;
    [SerializeField] private TMP_Text _lvlText;
    private bool _isPressed = false;

    private bool _gameActive = false;
    private int _sliderDirection = 1;

    private void Awake()
    {
        _gamePanel.SetActive(false);
        _pressButton.onClick.AddListener(OnPress);
        _exitButton.onClick.AddListener(Exit);
        _prize.gameObject.SetActive(false);
    }

    public void StartGame(List<CharacterProgressData> character)
    {
        _gamePanel.SetActive(true);
        _prize.gameObject.SetActive(false);
        _timer = _gameDuration;
        _slider.value = 0.5f;
        _isPressed = true;
        CharactersEnum id = character[0].CharacterID;
        _characterImage.sprite = CharacterDatabase.Instance.GetCharacterData(id).Icon;
        _lvlText.text = character[0].LvlBalk.ToString();

        if (_greenZone != null)
        {
            float startHelper = 1f;

            if (character[0].LvlBalk < 3)
                startHelper = 1.4f;

            float newWidth = LvlCoefficient * character[0].LvlBalk * startHelper;
            _greenZone.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, newWidth);
        }

        _resultPointer.localRotation = Quaternion.Euler(0, 0, 0);
        _spinAngle = 0f;

        _gameActive = true;
        _pressButton.interactable = true;
        _scoreText.text = _winScore.ToString();

        StopAllCoroutines();
        StartCoroutine(UpdateGame());
    }

    private IEnumerator UpdateGame()
    {
        while (_gameActive)
        {
            // обновление таймера
            _timer -= Time.deltaTime;
            _timerText.text = Mathf.Clamp(_timer,0, _gameDuration).ToString("0.0");

            _pressButton.interactable = _isPressed;

            if (_timer <= 0)
            {
                EndGame();
                yield break;
            }

            if (IsPointerInGreenZone())
                _timerText.color = Color.green;
            else
                _timerText.color = Color.white;

            _slider.value += _sliderMoveSpeed * Time.deltaTime * _sliderDirection;

            if (_slider.value >= 1f)
            {
                _slider.value = 1f;
                _sliderDirection = -1;
                _isPressed = true;
            }
            else if (_slider.value <= 0f)
            {
                _slider.value = 0f;
                _sliderDirection = 1;
                _isPressed = true;
            }

            _spinAngle -= ResistanceOpponent;
            UpdateResultPointer();

            yield return null;
        }
    }

    void OnPress()
    {
        if (!_gameActive) return;

        if (IsPointerInGreenZone())
            _spinAngle += _moveStep;
        else
            _spinAngle -= _moveStep;

        _isPressed = false;
        UpdateResultPointer();
    }

    private void UpdateResultPointer()
    {
        _spinAngle = Mathf.Clamp(_spinAngle, -45f, 45f);
        _resultPointer.localRotation = Quaternion.Euler(0, 0, _spinAngle);
    }

    void EndGame()
    {
        _gameActive = false;
        _pressButton.interactable = false;

        int score = _spinAngle > 0 ? _winScore : _loseScore;
        _prize.gameObject.SetActive(true);
        _prize.ShowPopup(score);
        _prize.OnClickAcceptButton += GiveReward;
    }

    void GiveReward(int score)
    {
        _prize.OnClickAcceptButton -= GiveReward;
        Progress.Instance.PlayerInfo.Score += score;
        Exit();
    }

    private bool IsPointerInGreenZone()
    {
        if (_sliderPointer == null || _greenZone == null)
        {
            Debug.LogError("SliderPointer or GreenZone is null");
            return false;
        }

        // ѕолучаем позиции в мировых координатах
        Vector3 pointerPos = _sliderPointer.transform.position;
        Vector3 greenZonePos = _greenZone.transform.position;

        // ѕолучаем ширину зеленой зоны
        float greenZoneWidth = _greenZone.rect.width * _greenZone.lossyScale.x;

        // ѕровер€ем, находитс€ ли указатель в пределах зеленой зоны
        return Mathf.Abs(pointerPos.x - greenZonePos.x) <= greenZoneWidth * 0.5f;
    }

    private void Exit()
    {
        _gamePanel.SetActive(false);
    }

}
