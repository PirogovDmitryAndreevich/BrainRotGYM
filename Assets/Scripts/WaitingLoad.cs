using System;
using System.Collections;
using UnityEngine;

[DefaultExecutionOrder(-200)]
public class WaitingLoad : MonoBehaviour
{
    public static WaitingLoad Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void WaitAndExecute(Func<bool> condition, Action method, float timeout = 10f)
    {
        StartCoroutine(WaitAndExecuteCoroutine(condition, method, timeout));
    }

    private IEnumerator WaitAndExecuteCoroutine(Func<bool> condition, Action method, float timeout)
    {
        float timer = 0f;

        while (!condition() && timer < timeout)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        if (condition())
        {
            method();
        }
        else
        {
            Debug.LogError($"{condition.Method.Name} = {condition}, {method.Method.Name} was not fulfilled");
        }
    }
}