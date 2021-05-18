using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMove : MonoBehaviour
{
    [SerializeField] float moveSpeed = 4028493;

    void Update()
    {
        transform.Translate(transform.forward * moveSpeed * Time.deltaTime);
    }
}
