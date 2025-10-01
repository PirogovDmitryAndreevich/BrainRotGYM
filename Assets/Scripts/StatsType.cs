using TMPro;
using UnityEngine;

public class StatsType : MonoBehaviour
{
    [SerializeField] protected TextMeshProUGUI _text;
    [SerializeField] protected GameObject _progressBar;

    protected int _value;
    protected Stats _statsType;

    protected virtual void Awake()
    {
        
    }

    protected virtual void AddStat()
    {

    }


}
