using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public MapGenerator map;
    private int[,] grid;
    private int gridSize;
    public float moveSpeed = 5f; // 控制移动速度
    private Vector3 moveDirection = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        gridSize = DataBus.Instance.gridSize;
        Init();
    }

    void Init()
    {
        grid = map.GenerateGrid();
        transform.position = new Vector3(gridSize - 2, 1, 0);
    }

    // Update is called once per frame
    void Update()
    {
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

        /*
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                // 记录触摸开始位置
                Vector3 startPosition = touch.position;
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                // 计算触摸结束位置和方向
                Vector2 endPosition = touch.position;
                Vector2 touchDirection = endPosition - touch.position;

                // 将 Vector2 转换为 Vector3
                Vector3 direction3D = new Vector3(touchDirection.x, touchDirection.y, 0);


                if (Mathf.Abs(touchDirection.x) > Mathf.Abs(touchDirection.y))
                {
                    // 水平方向
                    if (touchDirection.x > 0)
                    {
                        moveDirection += Vector3.right;
                    }
                    else
                    {
                        moveDirection += Vector3.left;
                    }
                }
                else
                {
                    // 垂直方向
                    if (touchDirection.y > 0)
                    {
                        moveDirection += Vector3.forward;
                    }
                    else
                    {
                        moveDirection += Vector3.back;
                    }
                }
            }
        }*/

        // 移动游戏对象
        if (moveDirection != Vector3.zero)
        {
            // 规范化方向向量并乘以速度和时间增量
            transform.Translate(moveDirection.normalized, Space.World);
        }

        // 重置移动方向
        moveDirection = Vector3.zero;
    }
}
