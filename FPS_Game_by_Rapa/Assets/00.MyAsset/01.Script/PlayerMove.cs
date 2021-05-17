using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] float moveSpeed = 1f, jumpPower = 5f;
    [SerializeField] float gravity = -9.81f;
    [SerializeField] bool isGround = false;

    float yVelocity = 0;

    CharacterController characterController;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        Move();
    }

    void Move()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 dir = new Vector3(h, 0, v);
        dir.Normalize();

        dir = Camera.main.transform.TransformDirection(dir);

        GroundCheck();

        if (Input.GetButtonDown("Jump") && isGround)
        {
            yVelocity = jumpPower;
            isGround = false;
        }

        yVelocity += gravity * Time.deltaTime;
        dir.y = yVelocity;

        characterController.Move(dir * moveSpeed * Time.deltaTime);
    }

    void GroundCheck() => isGround = characterController.collisionFlags == CollisionFlags.Below ? true : false;
}