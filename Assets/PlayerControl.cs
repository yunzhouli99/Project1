using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public MapGenerator map;
    private int[,] grid;
    private int gridSize;
    public float moveSpeed = 5f; // �����ƶ��ٶ�
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
        // ����������
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
                // ��¼������ʼλ��
                Vector3 startPosition = touch.position;
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                // ���㴥������λ�úͷ���
                Vector2 endPosition = touch.position;
                Vector2 touchDirection = endPosition - touch.position;

                // �� Vector2 ת��Ϊ Vector3
                Vector3 direction3D = new Vector3(touchDirection.x, touchDirection.y, 0);


                if (Mathf.Abs(touchDirection.x) > Mathf.Abs(touchDirection.y))
                {
                    // ˮƽ����
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
                    // ��ֱ����
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

        // �ƶ���Ϸ����
        if (moveDirection != Vector3.zero)
        {
            // �淶�����������������ٶȺ�ʱ������
            transform.Translate(moveDirection.normalized, Space.World);
        }

        // �����ƶ�����
        moveDirection = Vector3.zero;
    }
}
