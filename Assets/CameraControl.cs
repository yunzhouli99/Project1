using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public Camera cam;
    public float oSize = 0.75f;

    // Start is called before the first frame update
    void Start()
    {
        if (cam == null)
        {
            cam = GetComponent<Camera>();
        }
        int gridSize = DataBus.Instance.gridSize;
        transform.position = new Vector3(gridSize * 0.5f - 0.5f, gridSize * 0.5f - 0.5f, -10);
        if (Screen.width >= Screen.height)
        {
            cam.orthographicSize = gridSize * oSize;
        }
        else
        {
            cam.orthographicSize = gridSize + 2;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
