using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MyPrefabs : MonoBehaviour
{
    public static MyPrefabs Instance;

    [SerializeField] public GameObject ScorePrefab;

    [SerializeField] public GameObject ClickEffect;

    private void Awake()
    {
        // ���������, ���������� �� ��� ���������
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // �����������, ���� ����� ��������� ����� �������
        }
        else
        {
            // ���� ��� ���������� ������ ���������, ���������� ����
            Destroy(gameObject);
        }
    }
}
