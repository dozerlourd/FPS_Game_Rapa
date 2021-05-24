using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFSM : MonoBehaviour
{
    CharacterController characterController;
    enum State
    {
        Idle,
        Trace,
        Attack,
        Damaged,
        Die
    }

    State EState;

    [SerializeField] float moveSpeed;
    [SerializeField] float traceRange, attackRange;

    Transform playerTr;

    
    void Start()
    {
        playerTr = GameObject.Find("Player").transform;
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        switch(EState)
        {
            case State.Idle:
                {
                    Idle();
                    break;
                }
            case State.Trace:
                {
                    Trace();
                    break;
                }
            case State.Attack:
                {
                    Attack();
                    break;
                }
            case State.Damaged:
                {
                    Damaged();
                    break;
                }
            case State.Die:
                {
                    Die();
                    break;
                }
        }
    }

    private void Idle()
    {
        float dist = (playerTr.position - transform.position).magnitude;
        
        if(dist <= traceRange)
        {
            EState = State.Trace;
        }
    }

    float d = 0;
    private void Trace()
    {
        Vector3 dir = playerTr.position - transform.position;
        float dist = dir.magnitude;
        dir.Normalize();
        d += Time.deltaTime;
        characterController.Move(dir * moveSpeed * Time.deltaTime);
        transform.rotation = Quaternion.LookRotation(new Vector3(Mathf.Lerp(transform.rotation.x, dir.x, d), dir.y, dir.z));

        if(dist <= attackRange)
        {
            //EState = State.Attack;
        }
    }

    private void Attack()
    {

    }
    private void Damaged()
    {

    }

    private void Die()
    {

    }

}
