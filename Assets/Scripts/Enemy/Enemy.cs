using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private NavMeshAgent _agent;
    [SerializeField] private Transform _walkPoint;

    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();

        _agent.SetDestination(_walkPoint.position);
    }
}
