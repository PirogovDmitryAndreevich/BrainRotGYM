using System.Collections;
using UnityEngine;

public class CircleMover : MonoBehaviour
{
    private RectTransform _rectTransform;
    private float _radius;
    private Vector2 _direction;

    private float _minX;
    private float _minY;
    private float _maxX;
    private float _maxY;

    public void Move(Circle circle, RectTransform gameZone, float speed)
    {
        StopAllCoroutines();
        _rectTransform = circle.Rect;
        _radius = circle.Radius;
        GetLocalCorners(gameZone);
        GenerationDirection();

        StartCoroutine(MoveCircleRoutine(_rectTransform, speed, _direction));
    }

    private void GenerationDirection()
    {
        Vector2 dir;

        do
        {
            dir = Random.insideUnitCircle.normalized;
        }
        while (Mathf.Abs(dir.x) < 0.1f && Mathf.Abs(dir.y) < 0.1f);

        _direction = dir;
    }


    private IEnumerator MoveCircleRoutine(RectTransform circle, float speed, Vector2 direction)
    {
        while (true)
        {
            // Движение
            circle.position += (Vector3)direction * speed * Time.deltaTime;

            Vector3 pos = circle.localPosition;

            // Проверка столкновения со стенами по X
            if (pos.x - _radius <= _minX || pos.x + _radius >= _maxX)
            {
                direction.x *= -1;   // отражение
                direction = AddRandomAngle(direction, 15f);
                pos.x = Mathf.Clamp(pos.x, _minX + _radius, _maxX - _radius);
            }

            // Проверка столкновения со стенами по Y
            if (pos.y - _radius <= _minY || pos.y + _radius >= _maxY)
            {
                direction.y *= -1;   // отражение
                direction = AddRandomAngle(direction, 15f);
                pos.y = Mathf.Clamp(pos.y, _minY + _radius, _maxY - _radius);
            }

            // Применяем корректированную позицию
            circle.localPosition = pos;

            yield return null;
        }
    }

    private Vector2 AddRandomAngle(Vector2 dir, float maxAngle)
    {
        float angle = Random.Range(-maxAngle, maxAngle);
        float rad = angle * Mathf.Deg2Rad;

        // поворот вектора на угол
        float cos = Mathf.Cos(rad);
        float sin = Mathf.Sin(rad);

        Vector2 newDir = new Vector2(
            dir.x * cos - dir.y * sin,
            dir.x * sin + dir.y * cos
        );

        return newDir.normalized;
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
}
