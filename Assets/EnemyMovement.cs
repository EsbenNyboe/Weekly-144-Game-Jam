using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    public float radius = 100;
    NavMeshAgent navAgent;

    Transform Player;

    public float hitThreshold = 1;
    public float detectPlayerThreshold = 30;

    bool patrolling;
    private void Start()
    {
        TryGetComponent(out navAgent);
        Player = GameObject.FindGameObjectsWithTag("Player")[0].transform;
        navAgent.stoppingDistance = hitThreshold;

        navAgent.destination = Player.position;
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, Player.position) < detectPlayerThreshold)
        {
            navAgent.SetDestination(Player.position);
            patrolling = false;

        }
        else if (Vector3.Distance(transform.position,Player.position) < hitThreshold)
        {
            //attack 
            patrolling = false;

        }
        else
        {
            Patrol();
            patrolling = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player hit");
        }
    }

    Vector3 newPatrolPoint()
    {
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        randomDirection += transform.position;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, 20, 1);
        return hit.position;
    }

    void Patrol()
    {
        if (!patrolling)
            navAgent.SetDestination(newPatrolPoint());

        if (navAgent.remainingDistance < hitThreshold)
        {
            navAgent.SetDestination(newPatrolPoint());
        }
    }
}
