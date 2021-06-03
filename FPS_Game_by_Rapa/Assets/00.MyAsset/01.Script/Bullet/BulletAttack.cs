using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletAttack : MonoBehaviour
{
    [SerializeField] GameObject effect;

    private void OnCollisionEnter(Collision col)
    {
        ContactPoint colContact = col.contacts[0];
        GameObject effectClone = Instantiate(effect,colContact.point, Quaternion.LookRotation(-colContact.normal));
        col.collider.GetComponent<EnemyFSM>().Damaged(5);
        Debug.Log("HA");
    }
}
