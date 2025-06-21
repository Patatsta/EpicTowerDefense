using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private NavMeshAgent _agent;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    private void OnEnable()
    {
        GameObject walkPointObj = GameObject.Find("WayPoint");
        if (walkPointObj != null)
        {
            _agent.enabled = false; 
            _agent.enabled = true;  
            _agent.SetDestination(walkPointObj.transform.position);
        }
    }
}

