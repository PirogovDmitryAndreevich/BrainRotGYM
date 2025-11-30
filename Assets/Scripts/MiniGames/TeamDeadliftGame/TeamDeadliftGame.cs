using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(FlyingUpScoreMiniGame))]
public class TeamDeadliftGame : MonoBehaviour, IMiniGames
{
    private MiniGamesType _type = MiniGamesType.TeamDeadliftGame;
    public MiniGamesType Type => _type;
    public int MaxCharacters => _maxCharacters;

    private const float LimitRotation = 15f;
    private const float MinTiltSpeed = 3f;
    private const float MaxTiltSpeed = 7f;
    private const int MinBenchLvl = 1;
    private const int MaxBenchLvl = 20;
    private const float AngeleTiltMin = 0.5f;
    private const int MinScoreBonus = 17;

    [Header("Game Settings")]
    [SerializeField] private GameObject _gamePanel;
    [SerializeField] private Button _exitButton;
    [SerializeField] private int _maxCharacters = 4;
    [SerializeField] private RectTransform _charactersIconsGrid;
    [SerializeField] private PrizePopup _prize;

    [Header("Balance Board")]
    [SerializeField] private Image _background;
    [SerializeField] private RectTransform _balanceBoard;
    [SerializeField] private Button _leftButton;
    [SerializeField] private Button _rightButton;
    [SerializeField] private float _moveStep = 2f;
    [SerializeField] private float _boardAngle;
    [SerializeField] private float _tiltSpeed = 7f;

    [Header("Tension")]
    [SerializeField] private Slider _tension;

    [Header("Result UI")]
    [SerializeField] private TMP_Text _timer;
    [SerializeField] private TMP_Text _resultScore;

    [Header("UI Elements")]
    [SerializeField] private TMP_Text _timerFor;
    [SerializeField] private TMP_Text _currentScore;

    [Header("Score prefab settings")]
    [SerializeField] private RectTransform _parentObject;
    [SerializeField] private float _duration;
    [SerializeField] private float _flyDistance;

    private float _totalHorizontalLvl;
    private float _totalBenchLvl;
    private bool _gameActive = false;
    private float _gameTimer;
    private int _score;
    private int _scoreBonus = 10;
    private float _nextScoreTime = 18f;
    private float _tiltDirection = 0f;
    private float _nextScoreTimeForText;
    private FlyingUpScoreMiniGame _flyingScore;
    private GameObject _scorePrefab;

    private void Awake()
    {
        _gamePanel.SetActive(false);
        _leftButton.onClick.AddListener(OnPressedLeft);
        _rightButton.onClick.AddListener(OnPressedRight);
        _exitButton.onClick.AddListener(Exit);
        _flyingScore = GetComponent<FlyingUpScoreMiniGame>();
        _prize.gameObject.SetActive(false);
    }


    public void StartGame(List<CharacterProgressData> characters)
    {
        AudioManager.Instance.PlayTeamDeadliftMusic();
        _gamePanel.SetActive(true);
        _totalHorizontalLvl = 0f;
        _totalBenchLvl = 0f;
        _boardAngle = 0f;
        _tension.value = 0f;
        _gameTimer = 0f;
        _score = 0;
        _prize.gameObject.SetActive(false);
        GameObject prefab = MyPrefabs.Instance.CharacterIcon;
        ClearIconsGrid();

        foreach (var character in characters)
        {
            _totalHorizontalLvl += character.LvlHorizontalBars;
            _totalBenchLvl += character.LvlBench;

            var icon = Instantiate(prefab, _charactersIconsGrid);

            var comp = icon.GetComponent<CharacterIconPrefab>();
            comp.SetIcon(character.CharacterID);
        }

        _scoreBonus = Mathf.Max((int)(_totalBenchLvl + _totalHorizontalLvl), MinScoreBonus);
        _tiltSpeed = Mathf.Lerp(MaxTiltSpeed, MinTiltSpeed, Mathf.InverseLerp(MinBenchLvl, MaxBenchLvl, _totalBenchLvl));
        _currentScore.text = _scoreBonus.ToString();
        _nextScoreTimeForText = _nextScoreTime;

        _gameActive = true;
        StopAllCoroutines();
        StartCoroutine(UpdateGame());
    }

    private IEnumerator UpdateGame()
    {
        while (_gameActive)
        {
            _gameTimer += Time.deltaTime;
            _nextScoreTimeForText -= Time.deltaTime;
            _timerFor.text = _nextScoreTimeForText.ToString("0.0");
            _timer.text = _gameTimer.ToString("0.0");
            _resultScore.text = _score.ToString();

            if (_gameTimer >= _nextScoreTime)
            {
                _nextScoreTimeForText = _nextScoreTime;
                _score += _scoreBonus;
                _nextScoreTime += _nextScoreTime;
                CreateScorePrefab();
            }

            if (_tension.value >= 1)
            {
                EndGame();
                yield break;
            }

            if (Mathf.Abs(_boardAngle) <= AngeleTiltMin)
            {
                _tiltDirection = Random.value > AngeleTiltMin ? 1f : -1f;
                _boardAngle += _tiltSpeed * _tiltDirection * Time.deltaTime;
            }
            else
            {
                _boardAngle += _tiltSpeed * _tiltDirection * Time.deltaTime;
            }

            UpdateBalanceBoard();

            if (Mathf.Abs(_boardAngle) > 2f)
            {
                float offCenter = Mathf.Abs(_boardAngle) / LimitRotation; // 0..1
                float tensionRate = 0.1f + offCenter * 0.4f; // чем больше отклонение, тем выше
                _tension.value += tensionRate * Time.deltaTime * 0.1f;
                _background.color = Color.Lerp(Color.green, Color.red, offCenter);
            }

            yield return null;
        }
    }

    private void UpdateBalanceBoard()
    {
        _boardAngle = Mathf.Clamp(_boardAngle, -LimitRotation, LimitRotation);
        _balanceBoard.localRotation = Quaternion.Euler(0, 0, _boardAngle);
    }

    private void OnPressedLeft()
    {
        SoundEffects.Instance.PlayClickButtonDeadlift();
        _boardAngle += _moveStep;
    }
    private void OnPressedRight()
    {
        SoundEffects.Instance.PlayClickButtonDeadlift();
        _boardAngle -= _moveStep;
    }

    void EndGame()
    {
        _gameActive = false;
        _prize.gameObject.SetActive(true);
        _prize.ShowPopup(_score);
        _prize.OnClickAcceptButton += GiveReward;
    }

    void GiveReward(int score)
    {
        _prize.OnClickAcceptButton -= GiveReward;
        Progress.Instance.PlayerInfo.Score += score;
        Exit();
    }

    private void Exit()
    {
        SoundEffects.Instance.PlayOpenPopupSelected();
        AudioManager.Instance.PlayGYMMusic();
        _gamePanel.SetActive(false);
    }

    private void CreateScorePrefab()
    {        
        SoundEffects.Instance.PlayCreateScore();
        _scorePrefab = Instantiate(MyPrefabs.Instance.ScoreEffect, _parentObject);
        _scorePrefab.GetComponent<ScorePrefabEffect>().SetScoreValue(_scoreBonus);
        _flyingScore.StartFlyingUp(_parentObject.position, _scorePrefab, _duration, _flyDistance);
    }

    private void ClearIconsGrid()
    {
        if (_charactersIconsGrid == null) return;

        foreach (Transform child in _charactersIconsGrid.transform)
        {
            if (child != null && child.gameObject != null)
                Destroy(child.gameObject);
        }
    }
}
