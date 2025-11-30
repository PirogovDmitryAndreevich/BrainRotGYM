using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class OpenedCharacterPanel : MonoBehaviour
{
    [SerializeField] private Transform _openedGroup;
    [SerializeField] private Transform _noOpenedGroup;
    [SerializeField] private ScrollRect _scrollRect;

    private bool _isContentFilled = false;

    private void Awake()
    {
        GameManager.Instance.OnAllSystemsReady += Initialize;
    }

    private void OnEnable()
    {
        if (_isContentFilled)
            ResetScrollPositionNextFrame();

        StartCoroutine(RefreshLayout());
    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.OnAllSystemsReady -= Initialize;

        if (CharactersDataManager.Instance != null)
            CharactersDataManager.Instance.OnNewCharacterIsOpened -= OnCharacterUnlocked;
    }

    private void Initialize()
    {
        if (_isContentFilled)
        {
            Debug.Log("OpenedCharacterPanel: контент уже заполнен, пропускаем повторную инициализацию");
            return;
        }

        CharactersDataManager.Instance.OnNewCharacterIsOpened += OnCharacterUnlocked;

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
        if (_isContentFilled)
        {
            Debug.Log("OpenedCharacterPanel: FillContent уже вызывался — пропуск");
            return;
        }

        CharactersEnum currentCharacter = Progress.Instance.PlayerInfo.CurrentCharacter.CharacterID;
        _isContentFilled = true;

        foreach (var kvp in CharacterDatabase.Instance.AllCharactersDictionary)
        {
            var characterID = kvp.Key;
            var prefab = MyPrefabs.Instance.SelectCharacterButton;

            Transform parent = OpenedCharactersManager.Instance.IsCharacterOpened(characterID)
                ? _openedGroup
                : _noOpenedGroup;

            GameObject go = Instantiate(prefab, parent);
            var component = go.GetComponent<SelectCharacterButtonComponent>();
            component.InitializeCharacterButtonSelect(characterID, CharacterSelectionController.Instance.SelectCharacter);

            /*if (characterID == currentCharacter)
                CharacterSelectionController.Instance.SelectCharacter(component);*/
        }

        ResetScrollPositionNextFrame();
        StartCoroutine(RefreshLayout());
    }

    private void OnCharacterUnlocked(CharactersEnum characterID)
    {
        // Находим кнопку в списке неоткрытых
        var button = _noOpenedGroup
            .GetComponentsInChildren<SelectCharacterButtonComponent>(true)
            .FirstOrDefault(c => c.CharacterID == characterID);

        if (button == null)
        {
            Debug.LogWarning($"Не удалось найти карточку персонажа {characterID} в закрытых");
            return;
        }

        //  Перемещаем объект в контейнер открытых
        button.transform.SetParent(_openedGroup, false);

        // Переинициализируем кнопку для отображения статы
        button.InitializeCharacterButtonSelect(characterID, CharacterSelectionController.Instance.SelectCharacter);

        //  (опционально) красивая анимация появления
        StartCoroutine(PlayUnlockAnimation(button));
        StartCoroutine(RefreshLayout());
    }

    private IEnumerator PlayUnlockAnimation(SelectCharacterButtonComponent button)
    {
        var rect = button.GetComponent<RectTransform>();
        var canvasGroup = button.GetComponent<CanvasGroup>() ?? button.gameObject.AddComponent<CanvasGroup>();

        // простая fade-in анимация
        float t = 0f;
        float duration = 0.3f;
        canvasGroup.alpha = 0f;
        rect.localScale = Vector3.one * 0.8f;

        while (t < duration)
        {
            t += Time.deltaTime;
            float progress = t / duration;
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, progress);
            rect.localScale = Vector3.Lerp(Vector3.one * 0.8f, Vector3.one, progress);
            yield return null;
        }

        canvasGroup.alpha = 1f;
        rect.localScale = Vector3.one;
    }

    private void ResetScrollPositionNextFrame()
    {
        if (_scrollRect != null)
            _scrollRect.verticalNormalizedPosition = 1f;
    }

    private IEnumerator RefreshLayout()
    {
        // ждём один кадр, пока Unity обновит layout
        yield return null;

        LayoutRebuilder.ForceRebuildLayoutImmediate(_openedGroup.GetComponent<RectTransform>());
        LayoutRebuilder.ForceRebuildLayoutImmediate(_noOpenedGroup.GetComponent<RectTransform>());

        var parent = _openedGroup.parent.GetComponent<RectTransform>();
        LayoutRebuilder.ForceRebuildLayoutImmediate(parent);

        // ещё один кадр — на обновление ScrollRect
        yield return null;

        if (_scrollRect != null)
            _scrollRect.verticalNormalizedPosition = 1f;
    }
}
