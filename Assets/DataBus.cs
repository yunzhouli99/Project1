using UnityEngine;

public class DataBus : MonoBehaviour
{
    // 单例实例
    private static DataBus _instance;

    // 确保单例的唯一性
    public static DataBus Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject obj = new GameObject("DataBus");
                _instance = obj.AddComponent<DataBus>();
                DontDestroyOnLoad(obj); // 保持对象在场景切换时不被销毁
            }
            return _instance;
        }
    }

    // 网格大小
    public int gridSize = 10;

    // 网格数组
    public int[,] gridArray;

    private void Awake()
    {
        // 确保不会有多个实例
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject); // 保持对象在场景切换时不被销毁
        }
        else if (_instance != this)
        {
            Destroy(gameObject); // 销毁多余的实例
        }

        // 初始化网格数组
        InitializeGridArray();
    }

    // 初始化网格数组
    private void InitializeGridArray()
    {
        if (gridArray == null || gridArray.GetLength(0) != gridSize || gridArray.GetLength(1) != gridSize)
        {
            gridArray = new int[gridSize, gridSize];
        }
    }
}