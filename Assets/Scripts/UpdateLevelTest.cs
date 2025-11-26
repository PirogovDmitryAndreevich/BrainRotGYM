using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateLevelTest : MonoBehaviour
{
    [SerializeField] private Button _button;
    [SerializeField] private UpdateLevelPopup _popup;

    [SerializeField] private int _level;
    [SerializeField] private int _newLevel;
    [SerializeField] private int _score;

    private void Awake()
    {
        _button.onClick.AddListener(() => _popup.OpenPopup(_level, _newLevel, _score));
    }
}
