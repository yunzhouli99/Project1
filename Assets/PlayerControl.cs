using System.Collections;
using System.Collections.Generic;
using TMPro;
using System;
using UnityEngine;
using UnityEngine.UI;
using Unity.Mathematics;

public class PlayerControl : MonoBehaviour
{
    public MapGenerator map;
    private int[,] grid;
    private int gridSize;
    public float moveSpeed = 0.025f; // 控制移动速度
    private Vector3 moveDirection = Vector3.zero;
    private Vector3 targetPosition = Vector3.zero;
    private int[] steps = new int[3];
    public int maxStep = 4;
    private System.Random random;
    private bool storageable = true;


    // 引用 UI Text 元素
    public Text storageText;
    public Text currentStepText;
    public Text nextStepText;

    // Start is called before the first frame update
    void Start()
    {
        gridSize = DataBus.Instance.gridSize;
        steps = DataBus.Instance.steps;
        Init();
    }

    void Init()
    {
        random = new System.Random();
        grid = map.GenerateGrid();
        transform.position = new Vector3(gridSize - 2, 1, 0);
        targetPosition = transform.position;
        steps[0] = 0;
        steps[1] = random.Next(1, maxStep + 1);
        steps[2] = random.Next(1, maxStep + 1);
        StepsUpdate();
    }

    void StepsUpdate()
    {
        storageText.text = "存储的步长: " + steps[0].ToString();
        currentStepText.text = "当前的步长: " + steps[1].ToString();
        nextStepText.text = "下一步的步长: " + steps[2].ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
        {
            transform.position = targetPosition;
            // 检测键盘输入
            if (Input.GetKeyDown(KeyCode.W))
            {
                moveDirection += Vector3.up;
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                moveDirection += Vector3.down;
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                moveDirection += Vector3.left;
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                moveDirection += Vector3.right;
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (storageable)
                {
                    int temp = steps[0];
                    if (temp == 0)
                    {
                        do
                        {
                            temp = random.Next(1, maxStep + 1);
                        } while (temp == steps[1]);
                    }
                    steps[0] = steps[1];
                    steps[1] = temp;
                    storageable = false;
                    StepsUpdate();
                }
            }
            if (moveDirection != Vector3.zero)
            {
                storageable = true;
                for (int i = 0; i < steps[1]; i++)
                {
                    targetPosition += moveDirection.normalized;
                    if (grid[(int)targetPosition.x, (int)targetPosition.y] == 1)
                    {
                        Init();
                    }
                }
                // 重置移动方向
                steps[1] = steps[2];
                steps[2] = random.Next(1, maxStep + 1);
                StepsUpdate();
                if (grid[(int)targetPosition.x, (int)targetPosition.y] == 2)
                {
                    Init();
                }
                moveDirection = Vector3.zero;
            }
        }
        else
        {
            // 移动游戏对象
            if (transform.position != targetPosition)
            {
                // 平滑移动到目标位置
                transform.position = Vector3.Lerp(transform.position, targetPosition, moveSpeed);
                //Debug.Log(Time.deltaTime * moveSpeed);
                // 规范化方向向量并乘以速度和时间增量
            }
        }
    }
}
