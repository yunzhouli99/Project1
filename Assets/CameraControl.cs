using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        int gridSize = DataBus.Instance.gridSize;
        transform.position = new Vector3(gridSize * 0.5f - 0.5f, gridSize * 0.5f - 0.5f, -10);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
