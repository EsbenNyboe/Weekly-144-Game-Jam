using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    public float radius = 100;
    NavMeshAgent navAgent;
    Enemy enemyScript;
    Transform Player;

    public float hitThreshold = 1;
    public float detectPlayerThreshold = 30;

    bool patrolling;

    private void Start()
    {
        TryGetComponent(out enemyScript);
        if (enemyScript!=null) hitThreshold = enemyScript.shootRange;

        TryGetComponent(out navAgent);
        Player = GameObject.FindGameObjectsWithTag("Player")[0].transform;
        //navAgent.stoppingDistance = hitThreshold;

        navAgent.destination = Player.position;
    }

    private void Update()
    {
        float playerDistance = Vector3.Distance(transform.position, Player.position);

        if (playerDistance < detectPlayerThreshold && playerDistance > hitThreshold)
        {
            navAgent.isStopped = false;

            navAgent.SetDestination(Player.position);
            patrolling = false;

        }
        else if (playerDistance <= hitThreshold)
        {
            //attack 
            navAgent.isStopped=true;
            patrolling = false;

        }
        else
        {
            Patrol();
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
        {
            patrolling = true;
            navAgent.SetDestination(newPatrolPoint());
        }

        if (navAgent.remainingDistance < 2)
        {
            navAgent.SetDestination(newPatrolPoint());

        }
    }
}
