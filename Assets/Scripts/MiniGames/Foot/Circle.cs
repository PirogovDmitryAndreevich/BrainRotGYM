using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button), typeof(Image), typeof(RectTransform))]
[RequireComponent(typeof(CircleSpawn), typeof(CircleMover))]
public class Circle : MonoBehaviour
{
    public RectTransform Rect => _rectTransform;
    public float Radius => _radius;

    private Button _button;
    private RectTransform _rectTransform;
    private Image _image;
    private CircleSpawn _spawn;
    private CircleMover _mover;
    private float _radius;
    private float _speed;
    private CircleSet _circleType;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _rectTransform = GetComponent<RectTransform>();
        _image = GetComponent<Image>();
        _spawn = GetComponent<CircleSpawn>();
        _mover = GetComponent<CircleMover>();
    }

    public void Initialize(float radius, CircleType type, Action<CircleType> onClickEvent,
        Vector2 position, RectTransform gameZone)
    {
        _circleType = FootMiniGame.Instance.Circles[type];
        SetType();
        _button.onClick.AddListener(() =>
            {
                onClickEvent?.Invoke(type);
                Destroy(gameObject);
            }
        );
        _radius = radius;
        SetCircleRadius(radius);
        StartCoroutine(InitializeRoutine(position, gameZone));
    }

    private IEnumerator InitializeRoutine(Vector2 position, RectTransform gameZone)
    {
        yield return _spawn.Spawn(_rectTransform, position);

        _mover.Move(this, gameZone, _speed);
    }

    private void SetCircleRadius(float radius)
    {
        float diameter = radius * 2f;
        _rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, diameter);
        _rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, diameter);
    }

    private void SetType()
    {
        _image.color = _circleType.Color;
        _speed = _circleType.Speed;
    }
}
