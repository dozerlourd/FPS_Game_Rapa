using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotate : MonoBehaviour
{
    Camera mainCamera;
    Vector3 cameraVec;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        cameraVec = new Vector3(0, mainCamera.transform.eulerAngles.y, 0);
        transform.eulerAngles = cameraVec;
    }
}
