using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CirclesController), typeof(CircleTypeSelector))]
public class FootMiniGame : MonoBehaviour, IMiniGames
{
    public static FootMiniGame Instance;

    private MiniGamesType _type = MiniGamesType.Foot;
    public MiniGamesType Type => _type;
    public int MaxCharacters => _maxCharacters;

    [Header("Game Settings")]
    [SerializeField] private GameObject _gamePanel;
    [SerializeField] private Button _exitButton;
    [SerializeField] private RectTransform _gameZone;
    [SerializeField] private PrizePopup _prize;
    [SerializeField] private RectTransform _parentForCircles;
    [SerializeField] private int _maxCharacters = 1;

    [Header("Game Parameters")]
    [SerializeField] private float _circlesSpaceTime = 5;
    [SerializeField] private float _gameDuration = 10;    
    [SerializeField] private int _plusScore = 10;    
    [SerializeField] private int _minusScore = 10;    
    [SerializeField] private float _plusTime = 10;    
    [SerializeField] private float _minusTime = 10;

    [Header("UI Elements")]
    [SerializeField] private TMP_Text _timerText;
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private CharacterIconPrefab _characterImage;

    [Header("Circles settings")]
    [SerializeField] private CircleSet[] CirclesType;

    public Dictionary<CircleType, CircleSet> Circles;

    private float _timer;
    private float _timeElapsed;
    private bool _isPlaying = false;
    private float _radius = 50;
    private int _winScore = 0;
    private CirclesController _circleController;
    private CircleTypeSelector _circleSelector;

    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);

        Instance = this;

        _circleController = GetComponent<CirclesController>();
        _circleSelector = GetComponent<CircleTypeSelector>();
        _exitButton.onClick.AddListener(Exit);

        Circles = new Dictionary<CircleType, CircleSet>();

        foreach (var circle in CirclesType)
        {
            if (!Circles.ContainsKey(circle.Type))
                Circles[circle.Type] = circle;
        }

        _gamePanel.SetActive(false);
    }

    public void StartGame(List<CharacterProgressData> characters)
    {
        StopAllCoroutines();
        ClearParent();
        _gamePanel.SetActive(true);
        _prize.gameObject.SetActive(false);
        _timer = _gameDuration;
        _radius = 50f;
        _winScore = 0;
        _timeElapsed = 0f;

        foreach (var character in characters)
        {
            _radius += character.LvlFoots;
            _characterImage.SetIcon(character.CharacterID);
        }

        _circleController.Initialize(_gameZone, _radius, _parentForCircles);
        _isPlaying = true;
        StartCoroutine(UpdateGame());
    }

    public void AddScore() => _winScore += _plusScore;
    public void AddTime() => _timer += _plusTime;
    public void MinusScore() => _winScore -= _minusScore;
    public void MinusTime() => _timer -= _minusTime; 

    private IEnumerator UpdateGame()
    {
        Debug.Log("Start coroutine UpdateGame");

        float circlesTime = _circlesSpaceTime;
        _circleController.Spawn(CircleType.Score);

        while (_isPlaying)
        {
            _timerText.text = _timer.ToString("0.0");
            _scoreText.text = _winScore.ToString();
            _timer -= Time.deltaTime;
            circlesTime -= Time.deltaTime;
            _timeElapsed += Time.deltaTime;

            if (circlesTime <= 0)
            {
                _circleController.Spawn(_circleSelector.GetCircleType(_timeElapsed, 60f));                
                circlesTime = _circlesSpaceTime;
            }

            if (_timer <= 0)
            {
                EndGame();
                yield break;
            }

            yield return null;
        }
    }

    private void EndGame()
    {
        _isPlaying = false;
        _prize.ShowPopup(_winScore);
        _prize.OnClickAcceptButton += GivePrize;
        ClearParent();
    }

    private void Exit()
    {
        StopAllCoroutines();
        _gamePanel.SetActive(false);
        ClearParent();
    }

    private void ClearParent()
    {
        foreach (Transform child in _parentForCircles)        
            Destroy(child.gameObject);        
    }

    private void GivePrize(int score)
    {
        _prize.OnClickAcceptButton -= GivePrize;
        Progress.Instance.PlayerInfo.Score += score;
        Exit();
    }
}
