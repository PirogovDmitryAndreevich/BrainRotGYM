using System;
using System.Collections;
using UnityEngine;

public class CirclesController : MonoBehaviour
{
    private float _minX;
    private float _minY;
    private float _maxX;
    private float _maxY;

    private RectTransform _parentObject;
    private RectTransform _gameZone;
    private float _radius;
    private Action<CircleType> onClickCircle;
    private FootMiniGame _miniGame;

    private void OnDestroy()
    {
        onClickCircle -= OnClickCircle;
    }

    public void Initialize(RectTransform gameZone, float radius, RectTransform parentForCircles)
    {
        _miniGame = FootMiniGame.Instance;
        _radius = radius;
        _parentObject = parentForCircles;
        _gameZone = gameZone;
        GetLocalCorners(gameZone);
        onClickCircle += OnClickCircle;
    }

    public void Spawn(CircleType type)
    {
        GameObject circle = Instantiate(MyPrefabs.Instance.Circle, _parentObject);
        Vector2 position = GenerationPosition();
        Circle comp = circle.GetComponent<Circle>();
        comp.Initialize(_radius, type, onClickCircle, position, _gameZone);
    }

    private Vector2 GenerationPosition()
    {
        float x = UnityEngine.Random.Range(_minX + _radius, _maxX - _radius);
        float y = UnityEngine.Random.Range(_minY + _radius, _maxY - _radius);

        return new Vector2(x, y);
    }

    private void GetLocalCorners(RectTransform gameZone)
    {
        Vector3[] corners = new Vector3[4];
        gameZone.GetLocalCorners(corners);

        _minX = corners[0].x;
        _maxX = corners[2].x;
        _minY = corners[0].y;
        _maxY = corners[1].y;
    }

    private void OnClickCircle(CircleType type)
    {
        switch (type)
        {
            case CircleType.Score:
                FootMiniGame.Instance.AddScore();
                break;
            case CircleType.MinusScore:
                FootMiniGame.Instance.MinusScore();
                break;
            case CircleType.Time:
                FootMiniGame.Instance.AddTime();
                break;
            case CircleType.MinusTime:
                FootMiniGame.Instance.MinusTime();
                break;
        }
    }
}
