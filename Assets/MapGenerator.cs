using UnityEngine;
using System;
using System.Collections.Generic;

public class MapGenerator : MonoBehaviour
{
    public GameObject gridPrefab; // ����Ԥ�Ƽ�
    public GameObject spikePrefab; // �ϰ���Ԥ�Ƽ�
    private int gridSize;
    private float spacing = 1.0f; // ���Ӽ��
    private int[,] gridArray; // ���ڴ洢������Ϣ�Ķ�ά����
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
        // ��ʼ����ά����
        gridArray = new int[gridSize, gridSize];

        // ���ñ�ԵΪ1
        GenerateSpike();

        // �����������ɸ��Ӻ��ϰ���
        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                Vector3 position = new Vector3(x * spacing, y * spacing, 0);

                // ʵ�������Ӳ�������Ϊ��ǰ���󣨼������󣩵��Ӷ���
                GameObject gridTile = Instantiate(gridPrefab, position, Quaternion.identity);
                gridTile.transform.SetParent(transform);

                // �����ǰ����ֵΪ1����ʵ����һ���ϰ���
                if (gridArray[x, y] == 1)
                {
                    GameObject spikeTile = Instantiate(spikePrefab, position, Quaternion.identity);
                    spikeTile.transform.SetParent(transform);
                }
            }
        }
        return gridArray;
    }

    // ���ö�ά����ı�ԵΪ1
    void GenerateSpike()
    {
        // ���õ�һ�к����һ��
        for (int x = 0; x < gridSize; x++)
        {
            gridArray[x, 0] = 1;
            gridArray[x, gridSize - 1] = 1;
        }

        // ���õ�һ�к����һ��
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
        
        // ����������Ƿ���ͨ
    }

    void CellularAutomata()
    {
        // ����һ�������������ʵ��
        System.Random random = new System.Random();
        for (int x = 1; x < gridSize - 1; x++)
        {
            for (int y = 1; y < gridSize - 1; y++)
            {
                // ����һ��0��1֮������������
                double randomValue = random.NextDouble();

                // ����pSpike�ĸ��ʾ������0����1
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

        // ��ʼ�����
        queue.Enqueue((startX, startY));
        visited[startX, startY] = true;

        // �ĸ������������ϡ��¡�����
        (int, int)[] directions = { (0, 1), (0, -1), (1, 0), (-1, 0) };

        while (queue.Count > 0)
        {
            var (currentX, currentY) = queue.Dequeue();

            // ����Ƿ񵽴�Ŀ���
            if (currentX == targetX && currentY == targetY)
            {
                return true;
            }

            // �����ĸ�����
            foreach (var (dx, dy) in directions)
            {
                int newX = currentX + dx;
                int newY = currentY + dy;

                // �����λ���Ƿ��������ڡ�δ���ʹ��Ҳ����ϰ���
                if (newX >= 0 && newX < gridSize && newY >= 0 && newY < gridSize &&
                    !visited[newX, newY] && gridArray[newX, newY] == 0)
                {
                    queue.Enqueue((newX, newY));
                    visited[newX, newY] = true;
                }
            }
        }

        // �������Ϊ����δ�ҵ�Ŀ��㣬�򷵻�false
        return false;
    }
}