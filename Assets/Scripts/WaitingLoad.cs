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

    public void WaitAndExecute(Func<bool> condition, Action method)
    {
        StartCoroutine(WaitAndExecuteCoroutine(condition, method));
    }

    private IEnumerator WaitAndExecuteCoroutine(Func<bool> condition, Action method)
    {
        yield return new WaitUntil(condition);
        method?.Invoke();
    }
}