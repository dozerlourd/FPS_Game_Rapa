using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageAction : MonoBehaviour
{
    EnemyFSM eFSM;
    PlayerMove playerMove;

    private void Awake()
    {
        eFSM = transform.parent.GetComponent<EnemyFSM>();
        playerMove = GameObject.Find("Player").GetComponent<PlayerMove>();
    }

    public void OnEnemyAttack(float _rate)
    {
        int finalDamage = (int)(eFSM.AttackPower * _rate);

        playerMove.CurrHP -= finalDamage;
        Debug.Log("나 피해 입었다 " + finalDamage.ToString() + " 으억");
    }
}
