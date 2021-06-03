using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 마우스 커서를 화면 중간 위치(에임)에 고정시켜주는 스크립트
/// </summary>
public class FixedAim : MonoBehaviour
{
    bool isFixed;
    public bool IsFixed
    {
        get => isFixed;
        set
        {
            isFixed = value;
            Cursor.lockState = IsFixed ? CursorLockMode.Locked : CursorLockMode.None;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            IsFixed = IsFixed == true ? false : true;
        }
    }
}
