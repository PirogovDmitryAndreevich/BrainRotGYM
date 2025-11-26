using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class CircleSpawn : MonoBehaviour
{
    [SerializeField] private float _duration = 0.2f;

    public IEnumerator Spawn(RectTransform circle, Vector2 position)
    {
        circle.localPosition = position;
        circle.localScale = Vector3.zero;

        float time = 0f;

        while (time < _duration)
        {
            time += Time.deltaTime;
            float t = time / _duration;

            // сглаживание
            float scale = Mathf.SmoothStep(0f, 1f, t);

            circle.localScale = new Vector3(scale, scale, 1f);

            yield return null;
        }

        circle.localScale = Vector3.one;
    }
}
