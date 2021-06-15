using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyFSM : MonoBehaviour
{
    NavMeshAgent myNav;
    CharacterController characterController;
    Animator anim;

    //데미지 상태에서 마지막 호출된 코루틴
    Coroutine c_damage;

    //상태 Enum문
    enum State
    {
        Idle = 0,
        Trace,
        Attack,
        Damaged,
        Die = 5
    }

    [SerializeField] State EState;

    [SerializeField] float moveSpeed = 0f;
    [SerializeField] float maxHP = 0f;
    [SerializeField] float traceRange = 0f, attackRange = 0f;
    [SerializeField] float attackDelay = 0f, attackPower = 0f;
    
    public float AttackPower => attackPower;

    Transform playerTr;

    float rate = 0, currHP;
    private float attackDelayTime, attackRangeCheck;

    void Start()
    {
        playerTr = GameObject.Find("Player").transform;
        characterController = GetComponent<CharacterController>();
        
        anim = GetComponentInChildren<Animator>();
        currHP = maxHP; // HP 초기화

        myNav = GetComponent<NavMeshAgent>();
        myNav.speed = moveSpeed;
        myNav.acceleration = 3f;
        myNav.stoppingDistance = attackRange;
    }

    void Update()
    {
        //Enum문으로 구성된 상태패턴 약식
        switch(EState)
        {
            case State.Idle:
                {
                    State_Idle();
                    break;
                }
            case State.Trace:
                {
                    //State_Trace();
                    State_Trace2();
                    break;
                }
            case State.Attack:
                {
                    State_Attack();
                    break;
                }
            case State.Damaged:
                {
                    if (isDamaged)
                    {
                        // c_damage가 null이 아니라면 기존의 c_damage를 멈추고 재호출한다.
                        if(c_damage != null) StopCoroutine(c_damage);
                        c_damage = StartCoroutine(Co_State_Damaged());
                    }
                    break;
                }
            case State.Die:
                {
                    State_Die();
                    break;
                }
        }
    }

    float idleTimer = 0;
    private void State_Idle()
    {
        //플레이어와 자신 사이의 거리
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

    private void State_Trace()
    {
        Vector3 dir = playerTr.position - transform.position;
        float dist = dir.sqrMagnitude;

        if (dist <= Mathf.Pow(attackRange,2))
        {
            anim.SetTrigger("MoveToAttack");
            SetState(State.Attack);
            attackDelayTime = 0;
        }
        
        if (dist >= Mathf.Pow(traceRange,2))
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

    void State_Trace2()
    {
        myNav.enabled = true;
        //플레이어의 위치를 NavMesh의 목적지로 설정한다.
        myNav.SetDestination(playerTr.position);

        float dist = (playerTr.position - transform.position).magnitude;

        if (dist <= attackRange)
        {
            anim.SetTrigger("MoveToAttack");
            SetState(State.Attack);
            attackDelayTime = 0;
            myNav.enabled = false;
        }

        if (dist >= traceRange)
        {
            anim.SetTrigger("MoveToIdle");
            idleTimer = 0;
            //Debug.Log("또넘어가욧");
            SetState(State.Idle);
            myNav.enabled = false;
        }
    }

    bool isBooked = false;
    private void State_Attack()
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
                isBooked = true;
            }
        }
    }

    IEnumerator AttackToTrace(float delay)
    {
        yield return new WaitForSeconds(delay);
        SetState(State.Trace);
        anim.SetTrigger("AttackToMove");
        isBooked = false;
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
        myNav.enabled = false;
        currHP = Mathf.Max(currHP - value, 0);
        if (EState == State.Die) return;

        if (currHP <= 0)
        {
            SetState(State.Die);
            anim.SetBool("IsDie", true);
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

    IEnumerator Co_State_Damaged()
    {
        float time;
        // 피격 애니메이션의 총 길이를 구한다.
        AnimatorStateInfo damagedInfo = anim.GetCurrentAnimatorStateInfo(0);
        while(true)
        {
            if (damagedInfo.IsName("Zombie_Idle"))
            {
                time = damagedInfo.length;
                //Debug.Log(time.ToString());
                break;
            }
            yield return new WaitForEndOfFrame();
        }

        isDamaged = false;
        yield return new WaitForSeconds(time/2);
        SetState(State.Idle);
        anim.ResetTrigger("IdleToMove");
    }

    void State_Die()
    {
        myNav.enabled = false;
        AnimatorStateInfo dieInfo = anim.GetCurrentAnimatorStateInfo(0);

        if(dieInfo.IsName("Zombie_Die"))
        {
            anim.SetBool("IsDie", false);

            if (dieInfo.normalizedTime >= 1f)
                StartCoroutine(Co_Die());
        }
    }

    IEnumerator Co_Die()
    {
        float rate = 0;
        while(rate < 1)
        {

            yield return new WaitForEndOfFrame();
        }
    }
}
