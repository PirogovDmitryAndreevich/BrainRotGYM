using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsUpdateDemonstration : MonoBehaviour, IDemonstration
{
    public void Demonstration()
    {
        UpdateManager.Instance.UpdateBodyView?.Invoke();
    }
}
