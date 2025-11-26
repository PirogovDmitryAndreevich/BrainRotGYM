using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpdateStatsPopup : MonoBehaviour
{
    [Header("UI settings")]
    [SerializeField] private GameObject _popup;
    [SerializeField] private Button _acceptButton;
    [SerializeField] private TMP_Text _lvlText;
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private Image _statsImage;

    [Header("Sprites")]
    [SerializeField] private Sprite _benchSprite;
    [SerializeField] private Sprite _horizontalSprite;
    [SerializeField] private Sprite _balksSprite;
    [SerializeField] private Sprite _footsSprite;

    [Header("Animation settings")]
    [SerializeField] private float _duration = 0.6f;
    [SerializeField] private float _overshoot = 20f;

    [Header("Editor")]
    [SerializeField] private int _newLevel;
    [SerializeField] private int _score;

    private Image _buttonImage;

    public Action<int> OnAcceptClick;

    private void Awake()
    {
        _popup.SetActive(false);
        _acceptButton.onClick.AddListener(Accept);
        _buttonImage = _acceptButton.GetComponent<Image>();
        _buttonImage.color = Color.white;
    }

    public void OpenPopup(int lvl, int newLel, int score, Stats type)
    {
        _lvlText.text = lvl.ToString();
        _scoreText.text = score.ToString();

        _newLevel = newLel;
        _score = score;

        switch (type)
        {
            case Stats.Bench:
                _statsImage.sprite = _benchSprite;
                break;
            case Stats.Balks:
                _statsImage.sprite = _balksSprite;
                break;
            case Stats.HorizontalBar:
                _statsImage.sprite = _horizontalSprite;
                break;
            case Stats.Foots:
                _statsImage.sprite = _footsSprite;
                break;
            default:
                throw new ArgumentException($"[{name}] Unknown stat type: {type}");
        }

        _acceptButton.interactable = false;
        _popup.SetActive(true);

        if (!this.isActiveAndEnabled)
        {
            Debug.LogError($"[{name}] This MonoBehaviour is not active/enabled, coroutine won't run.");
            return;
        }

        StartCoroutine(SwitchLvlAnimation());
    }

    private IEnumerator SwitchLvlAnimation()
    {
        Debug.Log("Coroutine is started");

        RectTransform rt = _lvlText.rectTransform;
        Vector3 startPos = _lvlText.rectTransform.localPosition;
        Vector3 downPos = new Vector3(0, -100, 0);
        Vector3 upPos = new Vector3(0, 100, 0);

        float elapsed = 0f;

        // ---------- 1. старая цифра уезжает вниз ----------
        Vector2 vel = Vector2.zero;
        while (elapsed < _duration / 2f)
        {
            elapsed += Time.deltaTime;
            rt.anchoredPosition = Vector2.SmoothDamp(rt.anchoredPosition, downPos, ref vel, 0.15f);
            yield return null;
        }

        // ---------- 2. меняем текст и ставим новую цифру сверху ----------
        _lvlText.text = _newLevel.ToString();
        rt.anchoredPosition = upPos;
        elapsed = 0f;
        vel = Vector2.zero;

        // ---------- 3. новая цифра плавно падает чуть ниже и "встряхивается" ----------
        Vector2 targetDown = new Vector2(0, -_overshoot);
        Vector2 targetUp = startPos;

        // опускаем чуть ниже
        while (elapsed < _duration / 2f)
        {
            elapsed += Time.deltaTime;
            rt.anchoredPosition = Vector2.SmoothDamp(rt.anchoredPosition, targetDown, ref vel, 0.1f);
            yield return null;
        }

        // короткий отскок обратно
        elapsed = 0f;
        vel = Vector2.zero;
        while (elapsed < _duration / 2f)
        {
            elapsed += Time.deltaTime;
            rt.anchoredPosition = Vector2.SmoothDamp(rt.anchoredPosition, targetUp, ref vel, 0.08f);
            yield return null;
        }

        // зафиксировать позицию
        rt.anchoredPosition = startPos;

        _acceptButton.interactable = true;
        _buttonImage.color = Color.green;
    }

    private void Accept()
    {
        _popup.SetActive(false);
        OnAcceptClick?.Invoke(_score);
    }
}
