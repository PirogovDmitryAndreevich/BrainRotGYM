using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class TestSelectCharacter : MonoBehaviour
{
    [SerializeField] private CharactersEnum _characterType;
    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        CharactersMenu.Instance.OnSelectCharacter( _characterType );
    }
}
