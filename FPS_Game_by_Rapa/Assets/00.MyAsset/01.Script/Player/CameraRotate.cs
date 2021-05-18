using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotate : MonoBehaviour
{
    [SerializeField] float rotSpeed, rotateLimitX;

    float rotX, rotY;
    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Mouse X");
        float y = Input.GetAxis("Mouse Y");

        #region
        //Vector3 dir = new Vector3(-y, x, 0);
        //dir.Normalize();

        //transform.eulerAngles += dir * rotSpeed * Time.deltaTime;

        //Vector3 limitDir = transform.eulerAngles;

        //limitDir.x = Mathf.Clamp(limitDir.x, -rotateLimitX, rotateLimitX);

        //transform.eulerAngles = limitDir;
        #endregion 안쓰는거

        rotX += x * rotSpeed * Time.deltaTime;
        rotY += y * rotSpeed * Time.deltaTime;

        rotY = Mathf.Clamp(rotY, -rotateLimitX, rotateLimitX);

        transform.eulerAngles = new Vector3(-rotY, rotX, 0);
    }
}