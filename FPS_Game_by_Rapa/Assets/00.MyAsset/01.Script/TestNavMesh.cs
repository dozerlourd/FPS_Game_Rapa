using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TestNavMesh : MonoBehaviour
{
    Transform targetTr;
    NavMeshAgent smith;

    void Start()
    {
        targetTr = GameObject.Find("Player").transform;
        smith = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        smith.SetDestination(targetTr.position);
    }
}
