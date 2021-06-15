using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] Transform firePos = null;
    [SerializeField] GameObject bullet = null;
    [SerializeField] GameObject bulletEffect = null;
    [SerializeField] float attackPower = 0f;
    Animator anim;

    private void Start()
    {
        anim = transform.GetChild(0).GetComponent<Animator>();
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(1))
        {
            Fire(bullet);
            anim.SetTrigger("Fire");
        }
        if (Input.GetMouseButtonDown(0))
        {
            RayFire();
            anim.SetTrigger("Fire");
        }
    }

    void Fire(GameObject _bullet)
    {
        GameObject bulletClone = Instantiate(_bullet);
        bulletClone.transform.position = firePos.position;
        bulletClone.transform.forward = firePos.forward;
        //bulletClone.transform.rotation = transform.rotation;
    }

    void RayFire()
    {
        Ray ray = new Ray(firePos.position, firePos.forward);
        Debug.DrawRay(firePos.position, firePos.forward * 100f, Color.green);
        RaycastHit rayHit;

        if(Physics.Raycast(ray, out rayHit))
        {
            //Debug.Log(rayHit.normal);

            GameObject SFX_Clone = Instantiate(bulletEffect, rayHit.point, Quaternion.LookRotation(-rayHit.normal));
            Debug.DrawRay(rayHit.point, rayHit.normal * 100, Color.red);

            if (rayHit.collider.tag == "Enemy")
            {
                rayHit.collider.GetComponent<EnemyFSM>().Damaged(attackPower);
            }


            #region 입사각과 반사각 구하는 공식
            //// Find the line from the gun to the point that was clicked.
            //Vector3 incomingVec = rayHit.point - Camera.main.transform.position;

            //// Use the point's normal to calculate the reflection vector.
            //Vector3 reflectVec = Vector3.Reflect(incomingVec, rayHit.normal);
            //// Draw lines to show the incoming "beam" and the reflection.
            //Debug.DrawLine(Camera.main.transform.position, rayHit.point, Color.red);
            //Debug.DrawRay(rayHit.point, reflectVec, Color.green);
            #endregion
        }
    }
}
