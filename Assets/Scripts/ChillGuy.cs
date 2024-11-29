using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChillGuy : MonoBehaviour
{
    private GameObject mainCamera;

    void Start()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
    }
    void Update()
    {
        Vector3 cameraPosition = mainCamera.transform.position;
        cameraPosition.y= transform.position.y;

        transform.LookAt(cameraPosition);
        transform.Rotate(0f, 180f, 0f);
    }
}
