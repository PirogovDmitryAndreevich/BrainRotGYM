using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HoverGlowButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject _glow;

    private void Awake()
    {
        _glow.GetComponent<Image>().color = Color.red;
        _glow.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _glow.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _glow.SetActive(false);
    }
}
