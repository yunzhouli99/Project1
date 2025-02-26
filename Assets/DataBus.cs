using UnityEngine;

public class DataBus : MonoBehaviour
{
    // ����ʵ��
    private static DataBus _instance;

    // ȷ��������Ψһ��
    public static DataBus Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject obj = new GameObject("DataBus");
                _instance = obj.AddComponent<DataBus>();
                DontDestroyOnLoad(obj); // ���ֶ����ڳ����л�ʱ��������
            }
            return _instance;
        }
    }

    // �����С
    public int gridSize = 10;

    // ��������
    public int[,] gridArray;

    private void Awake()
    {
        // ȷ�������ж��ʵ��
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject); // ���ֶ����ڳ����л�ʱ��������
        }
        else if (_instance != this)
        {
            Destroy(gameObject); // ���ٶ����ʵ��
        }

        // ��ʼ����������
        InitializeGridArray();
    }

    // ��ʼ����������
    private void InitializeGridArray()
    {
        if (gridArray == null || gridArray.GetLength(0) != gridSize || gridArray.GetLength(1) != gridSize)
        {
            gridArray = new int[gridSize, gridSize];
        }
    }
}