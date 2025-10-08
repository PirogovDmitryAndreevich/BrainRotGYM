using System.Collections;
using UnityEngine;

public class MoveToPosition : MonoBehaviour
{
    public IEnumerator Move(Transform targetTransform, Vector3 targetPosition, float duration)
    {
        Vector3 startPosition = targetTransform.position;
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            float progress = time / duration;

            float smoothProgress = Mathf.SmoothStep(0f, 1f, progress);

            targetTransform.position = Vector3.Lerp(startPosition, targetPosition, smoothProgress);
            yield return null;
        }

        targetTransform.position = targetPosition;
    }
}
