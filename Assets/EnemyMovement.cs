using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    public float radius = 20;
    NavMeshAgent navAgent;
    Enemy enemyScript;
    Transform Player;

    public float hitThreshold = 1;
    public float detectPlayerThreshold = 30;

    bool patrolling;

    public bool meleeEnemy;


    public delegate void Move();
    Move movement;
    public Transform eattackPoint;
    public float attackRange = 0.5f;
    public int attackDamage = 40;
    public LayerMask playerLayers;

    private void Start()
    {
        TryGetComponent(out enemyScript);

        TryGetComponent(out navAgent);
        Player = GameObject.FindGameObjectsWithTag("Player")[0].transform;
        //navAgent.stoppingDistance = hitThreshold;

        navAgent.destination = Player.position;

        if (meleeEnemy)
            movement = MeleeMovement;
        else
            movement = RangeMovement;
    }

    void RangeMovement()
    {
        float playerDistance = Vector3.Distance(transform.position, Player.position);
        bool playerStunned = enemyScript.playerScript.stun;

        if (playerDistance > detectPlayerThreshold)
        {
            Patrol();
            return;
        }

        if (playerDistance < detectPlayerThreshold)
        {
            navAgent.isStopped = false;

            navAgent.SetDestination(Player.position);
            patrolling = false;

        }
        if (enemyScript.inShootingRange())
        {
            navAgent.isStopped = true;
            patrolling = false;

        }
        else if (playerDistance <= hitThreshold)
        {
            navAgent.isStopped = true;
            patrolling = false;

            //melee attack method
        }

        
    }
    
    void MeleeMovement()
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
            navAgent.isStopped = true;
            patrolling = false;

            //melee attack method
            Collider[] hitPlayer = Physics.OverlapSphere(eattackPoint.position, attackRange, playerLayers);
            
            foreach (Collider player in hitPlayer)
            {
                Debug.Log("Enemy hit " + player.name);
                //enemy.GetComponent<Enemy>().TakeDamage(attackDamage);
            }
        }
        else
        {
            Patrol();
        }
    }
    
    void OnDrawGizmosSelected()
    {
        if (eattackPoint == null)
            return;

        Gizmos.DrawWireSphere(eattackPoint.position, attackRange);
    }

    private void Update()
    {
        movement?.Invoke();
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
            navAgent.isStopped = false;

            navAgent.SetDestination(newPatrolPoint());
        }

        if (navAgent.remainingDistance < 2)
        {
            navAgent.SetDestination(newPatrolPoint());

        }
    }
}
