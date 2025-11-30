using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniGamesManager : MonoBehaviour
{
    public static MiniGamesManager Instance;

    [SerializeField] private GameObject[] _games;
    [SerializeField] private Transform _gridForCharacters;
    [SerializeField] private Button _startMiniGameButton;

    private Dictionary<MiniGamesType, IMiniGames> _gamesDictionary = new Dictionary<MiniGamesType, IMiniGames>();
    private Dictionary<CharactersEnum, CharacterProgressData> _selectedCharacters = new();
    private IMiniGames _currentMiniGame;
    private IMiniGamesButton _currentMiniGameButton;
    private int _maxSelectCharacters;
    private Image _startButtonImage;

    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);

        Instance = this;
        DontDestroyOnLoad(gameObject);

        GameManager.Instance.OnAllSystemsReady += Initialize;
        _startMiniGameButton.onClick.AddListener(StartMiniGame);
        _startMiniGameButton.interactable = false;
        _startButtonImage = _startMiniGameButton.GetComponent<Image>();
    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.OnAllSystemsReady -= Initialize;
    }

    public void SelectMiniGame(MiniGamesType type, IMiniGamesButton button)
    {
        _selectedCharacters.Clear();

        foreach (Transform child in _gridForCharacters)
        {
            if (child.TryGetComponent<ISelectableCharacter>(out var charButton))
                charButton.Deselect();
        }

        if (_currentMiniGame == null && _currentMiniGameButton == null)
        {
            _currentMiniGame = _gamesDictionary[type];
            _currentMiniGameButton = button;
            _maxSelectCharacters = _currentMiniGame.MaxCharacters;
        }
        else
        {
            _currentMiniGame = _gamesDictionary[type];
            _maxSelectCharacters = _currentMiniGame.MaxCharacters;

            _currentMiniGameButton.Deselect();
            _currentMiniGameButton = button;
        }

        _currentMiniGameButton.Select();
        _gridForCharacters.gameObject.SetActive(true);
    }

    private void Initialize()
    {
        Debug.Log("MiniGamesManager Initialize called");

        foreach (var game in _games)
        {
            var g = game.GetComponent<IMiniGames>();
            _gamesDictionary[g.Type] = g;
        }

        if (OpenedCharactersManager.Instance.IsLoadIsComplete)
            FillContent();
        else
            WaitingLoad.Instance.WaitAndExecute
                (
                    () => OpenedCharactersManager.Instance.IsLoadIsComplete == true,
                    () => FillContent()
                );
    }

    private void FillContent()
    {
        if (OpenedCharactersManager.Instance == null)
        {
            Debug.LogError("OpenedCharactersManager.Instance is null!");
            return;
        }

        if (MyPrefabs.Instance == null)
        {
            Debug.LogError("MyPrefabs.Instance is null!");
            return;
        }

        foreach (var kvp in OpenedCharactersManager.Instance.OpenedCharacters)
        {
            var characterID = kvp.Key;
            var prefab = MyPrefabs.Instance.SelectCharacterButton;

            if (prefab == null)
            {
                Debug.LogError("SelectCharacterButton prefab is null!");
                continue;
            }

            GameObject go = Instantiate(prefab, _gridForCharacters);
            var component = go.GetComponent<SelectCharacterButtonComponent>();

            if (component != null)
                component.InitializeCharacterButtonSelect(characterID, SelectCharacters);
            else
                Debug.LogError("SelectCharacterButtonComponent not found on prefab!");
        }

        _gridForCharacters.gameObject.SetActive(false);
    }

    private void SelectCharacters(ISelectableCharacter characterButton)
    {
        if (!_selectedCharacters.ContainsKey(characterButton.CharacterID)
            && _selectedCharacters.Count < _maxSelectCharacters)
        {
            _selectedCharacters[characterButton.CharacterID] =
            OpenedCharactersManager.Instance.OpenedCharacters[characterButton.CharacterID];
            characterButton.Select();
        }
        else if (_selectedCharacters.ContainsKey(characterButton.CharacterID))
        {
            _selectedCharacters.Remove(characterButton.CharacterID);
            characterButton.Deselect();
        }

        if (_selectedCharacters.Count > 0)
        {
            _startButtonImage.color = Color.green;
            _startMiniGameButton.interactable = true;
        }
        else
        {
            _startButtonImage.color = Color.white;
            _startMiniGameButton.interactable = false;
        }
    } 

    private void StartMiniGame()
    {
        SoundEffects.Instance.PlayStartTournament();
        var selectedCharacters = new List<CharacterProgressData>(_selectedCharacters.Values);
        _currentMiniGame.StartGame(selectedCharacters);
        ResetSelects();
    }

    private void ResetSelects()
    {
        _currentMiniGameButton.Deselect();       
        DeselectCharacters();
        UpdateStartButtonState();
    }

    private void DeselectCharacters()
    {
        foreach (Transform child in _gridForCharacters)
        {
            if (child.TryGetComponent<ISelectableCharacter>(out var charButton))
                charButton.Deselect();
        }

        _selectedCharacters.Clear();
    }

    private void UpdateStartButtonState()
    {
        bool hasCharacters = _selectedCharacters.Count > 0;
        _startMiniGameButton.interactable = hasCharacters;
        _startButtonImage.color = hasCharacters ? Color.green : Color.white;
    }
}
