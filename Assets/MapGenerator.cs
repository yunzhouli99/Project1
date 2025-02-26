using UnityEngine;
using System;
using System.Collections.Generic;

public class MapGenerator : MonoBehaviour
{
    public GameObject gridPrefab; // 格子预制件
    public GameObject spikePrefab; // 障碍物预制件
    private int gridSize;
    private float spacing = 1.0f; // 格子间距
    private int[,] gridArray; // 用于存储网格信息的二维数组
    public double pSpike = 0.4;
    public int iteration = 3;
    public int bSpike = 6;
    public int sSpike = 3;

    void Start()
    {
        //GenerateGrid();
    }

    public int[,] GenerateGrid()
    {
        gridSize = DataBus.Instance.gridSize;
        gridArray = DataBus.Instance.gridArray;
        // 初始化二维数组
        gridArray = new int[gridSize, gridSize];

        // 设置边缘为1
        GenerateSpike();

        // 遍历网格并生成格子和障碍物
        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                Vector3 position = new Vector3(x * spacing, y * spacing, 0);

                // 实例化格子并将其作为当前对象（即父对象）的子对象
                GameObject gridTile = Instantiate(gridPrefab, position, Quaternion.identity);
                gridTile.transform.SetParent(transform);

                // 如果当前格子值为1，则实例化一个障碍物
                if (gridArray[x, y] == 1)
                {
                    GameObject spikeTile = Instantiate(spikePrefab, position, Quaternion.identity);
                    spikeTile.transform.SetParent(transform);
                }
            }
        }
        return gridArray;
    }

    // 设置二维数组的边缘为1
    void GenerateSpike()
    {
        // 设置第一行和最后一行
        for (int x = 0; x < gridSize; x++)
        {
            gridArray[x, 0] = 1;
            gridArray[x, gridSize - 1] = 1;
        }

        // 设置第一列和最后一列
        for (int y = 0; y < gridSize; y++)
        {
            gridArray[0, y] = 1;
            gridArray[gridSize - 1, y] = 1;
        }
        do
        {
            CellularAutomata();
            for (int y = 1; y < 5; y++)
            {
                gridArray[gridSize - 2, y] = 0;
            }
        } while (!ArePointsConnected(gridSize - 2, 1, 1, gridSize - 2));
        
        // 检查两个点是否连通
    }

    void CellularAutomata()
    {
        // 创建一个随机数生成器实例
        System.Random random = new System.Random();
        for (int x = 1; x < gridSize - 1; x++)
        {
            for (int y = 1; y < gridSize - 1; y++)
            {
                // 生成一个0到1之间的随机浮点数
                double randomValue = random.NextDouble();

                // 根据pSpike的概率决定输出0还是1
                gridArray[x, y] = (randomValue < pSpike) ? 1 : 0;
            }
        }
        for (int i = 0; i < iteration; i++)
        {
            IterateGrid();
        }
    }

    void IterateGrid()
    {
        int[,] newGridArray = new int[gridSize, gridSize];
        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                newGridArray[x, y] = gridArray[x, y];
            }
        }
        for (int x = 1; x < gridSize - 1; x++)
        {
            for (int y = 1; y < gridSize - 1; y++)
            {
                int surroundSpike = -gridArray[x, y];
                for (int a = x - 1; a <= x + 1; a++)
                {
                    for (int b = y - 1; b <= y + 1; b++)
                    {
                        surroundSpike += gridArray[x, y];
                    }
                }
                if (surroundSpike < sSpike)
                {
                    newGridArray[x, y] = 0;
                }
                if (surroundSpike > bSpike)
                {
                    newGridArray[x, y] = 1;
                }
            }
        }
        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                gridArray[x, y] = newGridArray[x, y];
            }
        }
    }

    bool ArePointsConnected(int startX, int startY, int targetX, int targetY)
    {
        bool[,] visited = new bool[gridSize, gridSize];
        Queue<(int, int)> queue = new Queue<(int, int)>();

        // 起始点入队
        queue.Enqueue((startX, startY));
        visited[startX, startY] = true;

        // 四个方向向量：上、下、左、右
        (int, int)[] directions = { (0, 1), (0, -1), (1, 0), (-1, 0) };

        while (queue.Count > 0)
        {
            var (currentX, currentY) = queue.Dequeue();

            // 检查是否到达目标点
            if (currentX == targetX && currentY == targetY)
            {
                return true;
            }

            // 遍历四个方向
            foreach (var (dx, dy) in directions)
            {
                int newX = currentX + dx;
                int newY = currentY + dy;

                // 检查新位置是否在网格内、未访问过且不是障碍物
                if (newX >= 0 && newX < gridSize && newY >= 0 && newY < gridSize &&
                    !visited[newX, newY] && gridArray[newX, newY] == 0)
                {
                    queue.Enqueue((newX, newY));
                    visited[newX, newY] = true;
                }
            }
        }

        // 如果队列为空且未找到目标点，则返回false
        return false;
    }
}