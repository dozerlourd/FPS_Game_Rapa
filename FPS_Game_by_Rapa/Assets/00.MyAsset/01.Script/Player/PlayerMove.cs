using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    Animator anim;

    [SerializeField] float runSpeed = 2f, jumpPower = 5f, walkSpeedRate;
    [SerializeField] float gravity = -9.81f;
    [SerializeField] bool isGround = false;
    [SerializeField] float maxHP, currHP;

    float moveSpeed = 0, moveSpeed_H = 0f, moveSpeed_V = 0f, walkSpeed = 0f, yVelocity = 0;

    public float CurrHP
    {
        get => currHP;
        set
        {
            currHP = value;
        }
    }

    CharacterController characterController;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        anim = transform.GetChild(0).GetComponent<Animator>();
        CurrHP = maxHP;
        walkSpeed = runSpeed * 0.35f;
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

        moveSpeed = (Input.GetKey(KeyCode.LeftShift) == true && h != 0) ? dir.magnitude * runSpeed : dir.magnitude * walkSpeed;

        dir = Camera.main.transform.TransformDirection(dir);

        GroundCheck();

        if (Input.GetButtonDown("Jump") && isGround)
        {
            yVelocity = jumpPower;
            isGround = false;
        }

        yVelocity += gravity * Time.deltaTime;
        dir.y = yVelocity;

        characterController.Move(new Vector3(dir.x * moveSpeed, dir.y, dir.z * moveSpeed) * Time.deltaTime);

        // 1D방식 moveSpeed 값 적용시켜주는 코드 필요
        //characterController.Move(new Vector3(dir.x * moveSpeed, dir.y, dir.z * moveSpeed) * Time.deltaTime);
        //anim.SetFloat("MoveSpeed_H", moveSpeed / runSpeed);

        anim.SetFloat("MoveSpeed_H", h);
        anim.SetFloat("MoveSpeed_V", v);
    }

    void GroundCheck() => isGround = characterController.collisionFlags == CollisionFlags.Below ? true : false;
}