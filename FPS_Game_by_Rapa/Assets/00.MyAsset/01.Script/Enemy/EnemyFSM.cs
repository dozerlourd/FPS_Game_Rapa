using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFSM : MonoBehaviour
{
    CharacterController characterController;
    Animator anim;

    Coroutine c_damage;

    enum State
    {
        Idle = 0,
        Trace,
        Attack,
        Damaged,
        Die = 5
    }

    [SerializeField] State EState;

    [SerializeField] float moveSpeed;
    [SerializeField] float maxHP;
    [SerializeField] float traceRange, attackRange;
    [SerializeField] float attackDelay, attackPower;
    
    public float AttackPower => attackPower;

    Transform playerTr;

    float rate = 0, currHP;
    private float attackDelayTime, attackRangeCheck;

    void Start()
    {
        playerTr = GameObject.Find("Player").transform;
        characterController = GetComponent<CharacterController>();
        
        anim = GetComponentInChildren<Animator>();
        currHP = maxHP;
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
                    if (isDamaged)
                    {
                        if(c_damage != null) StopCoroutine(c_damage);
                        c_damage = StartCoroutine(Damaged());
                    }
                    break;
                }
            case State.Die:
                {
                    Die();
                    break;
                }
        }
    }

    float idleTimer = 0;
    private void Idle()
    {
        float dist = (playerTr.position - transform.position).magnitude;

        if(dist <= traceRange && idleTimer >= 0.5f)
        {
            SetState(State.Trace);
            anim.SetTrigger("IdleToMove");
            idleTimer = 0;
            //Debug.Log("넘어가욧");
        }
        else
        {
            idleTimer += Time.deltaTime;
        }
    }

    private void Trace()
    {
        Vector3 dir = playerTr.position - transform.position;
        float dist = dir.magnitude;

        if (dist <= attackRange)
        {
            anim.SetTrigger("MoveToAttack");
            SetState(State.Attack);
            attackDelayTime = 0;
        }
        
        if (dist >= traceRange)
        {
            anim.SetTrigger("MoveToIdle");
            idleTimer = 0;
            //Debug.Log("또넘어가욧");
            SetState(State.Idle);
        }

        dir.Normalize();
        rate += Time.deltaTime;
        characterController.Move(dir * moveSpeed * Time.deltaTime);

        //Quaternion lookRotation = Quaternion.LookRotation(
        //new Vector3(Mathf.Lerp(transform.rotation.x, dir.x, rate), dir.y, dir.z));
        //transform.rotation = lookRotation;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), rate);
    }

    bool isBooked = false;
    private void Attack()
    {
        attackRangeCheck = Vector3.Distance(transform.position, playerTr.position);

        if (attackRangeCheck <= attackRange)
        {
            if (attackDelayTime >= attackDelay)
            {
                anim.SetTrigger("DelayToAttack");
                attackDelayTime = 0;
            }
            else
            {
                attackDelayTime += Time.deltaTime;
            }
        }
        else
        {
            if (!isBooked)
            {
                StartCoroutine(AttackToTrace(1.0f));
            }
        }
    }

    IEnumerator AttackToTrace(float delay)
    {
        yield return new WaitForSeconds(delay);
        SetState(State.Trace);
        anim.SetTrigger("AttackToMove");
    }

    void SetState(State value)
    {
        EState = value;
    }

    [SerializeField] bool isDamaged = false;
    /// <summary> 플레이어가 접근해서 해당 Enemy 객체에 대미지를 줄 때 사용되는 함수 </summary>
    /// <param name="value"> 대미지 값 </param>
    public void Damaged(float value)
    {
        currHP = Mathf.Max(currHP - value, 0);
        if (EState == State.Die) return;

        if (currHP <= 0)
        {
            SetState(State.Die);
            anim.SetTrigger("Die");
            characterController.enabled = false;
        }
        else
        {
            anim.SetTrigger("Damaged");
            if (EState == State.Damaged) return;
            isDamaged = true;
            SetState(State.Damaged);
        }
    }

    IEnumerator Damaged()
    {
        float time;
        // 피격 애니메이션의 총 길이를 구한다.
        AnimatorStateInfo damagedInfo = anim.GetCurrentAnimatorStateInfo(0);
        while(true)
        {
            if (damagedInfo.IsName("Zombie_Idle"))
            {
                time = damagedInfo.length;
                Debug.Log(time.ToString());
                break;
            }
            yield return new WaitForEndOfFrame();
        }

        isDamaged = false;
        yield return new WaitForSeconds(time/2);
        SetState(State.Idle);
        anim.ResetTrigger("IdleToMove");
    }

    void Die()
    {
        AnimatorStateInfo dieInfo = anim.GetCurrentAnimatorStateInfo(0);

        if(dieInfo.IsName("Zombie_Die") && dieInfo.normalizedTime >= 0.99f)
        {
            StartCoroutine(Co_Die());
        }
    }

    IEnumerator Co_Die()
    {
        float rate = 0;
        while(rate < 1)
        {
            
        }
    }
}
