using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;

[RequireComponent(typeof(CanvasGroup))]
public class PrizePopup : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI _score;
    [SerializeField] private Button _acceptButton;
    [SerializeField] private Button _rewardButton;

    [Header("Animation Settings")]
    [SerializeField] private float _scaleDuration = 0.2f;
    [SerializeField] private float _fadeDuration = 0.3f;

    public Action<int> OnClickAcceptButton;

    private CanvasGroup _canvasGroup;
    private RectTransform _rectPanel;
    private bool _isOpen = false;
    private int _finalScore;

    private void Awake()
    {
        _acceptButton.onClick.AddListener(OnClickAcceptPrize);
        _rewardButton.onClick.AddListener(OnCLickRewardButton);
        _canvasGroup = GetComponent<CanvasGroup>();
        _rectPanel = GetComponent<RectTransform>();

        _canvasGroup.alpha = 0;
    }

    public void ShowPopup(int score)
    {
        if (_isOpen == true) return;

        SoundEffects.Instance.PlayApplause();

        _score.text = score.ToString();
        _finalScore = score;
        _isOpen = true;
        _rewardButton.interactable = true;
        StopAllCoroutines();
        StartCoroutine(ShowPopupCoroutine());
    }

    private void OnCLickRewardButton()
    {
        SoundEffects.Instance.PlayOpenPopupSelected();

        string id = "X2";
        YG2.RewardedAdvShow(id, GiveReward);
    }

    private void GiveReward()
    {
        _rewardButton.interactable = false;
        _finalScore *= 2;
        _score.text = _finalScore.ToString();
    }

    private void OnClickAcceptPrize()
    {
        if (_isOpen == false) return;

        SoundEffects.Instance.PlayOpenPopupSelected();
        Debug.Log($"Игрок получил {_finalScore} очков!");
        StopAllCoroutines();
        StartCoroutine(HidePopupCoroutine());
        OnClickAcceptButton?.Invoke(_finalScore);
        _isOpen = false;
    }

    private IEnumerator ShowPopupCoroutine()
    {
        _canvasGroup.blocksRaycasts = true;

        // Анимация появления
        float elapsedTime = 0f;
        Vector3 startScale = Vector3.zero;
        Vector3 endScale = Vector3.one;

        while (elapsedTime < _fadeDuration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            float progress = elapsedTime / _fadeDuration;

            // Фейд
            _canvasGroup.alpha = Mathf.Lerp(0f, 1f, progress);

            // Скейл
            _rectPanel.localScale = Vector3.Lerp(startScale, endScale, progress / _scaleDuration);

            yield return null;
        }

        _canvasGroup.alpha = 1f;
        _rectPanel.localScale = endScale;
    }

    private IEnumerator HidePopupCoroutine()
    {
        _canvasGroup.blocksRaycasts = false;

        float elapsedTime = 0f;
        float startAlpha = _canvasGroup.alpha;

        while (elapsedTime < _fadeDuration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            float progress = elapsedTime / _fadeDuration;
            _canvasGroup.alpha = Mathf.Lerp(startAlpha, 0f, progress);
            yield return null;
        }

        _canvasGroup.alpha = 0f;
    }
}
