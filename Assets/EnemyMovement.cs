
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
    PlayerStats playerHP;

    public float hitThreshold = 1;
    public float detectPlayerThreshold = 30;

    bool patrolling;

    public bool meleeEnemy;

    public delegate void Move();
    Move movement;
    public Transform eattackPoint;
    public float attackRange = 0.5f;
    public int attackDamage = 1;
    public float attackDelay = 1;

    public LayerMask playerLayers;
    Animator anim;
    public Vector3 initialDestination;

    bool canAttack;

    private void Start()
    {
        TryGetComponent(out enemyScript);
        TryGetComponent(out navAgent);
        anim = enemyScript.anim;
        Player = GameObject.FindGameObjectsWithTag("Player")[0].transform;
        playerHP = Player.GetComponent<PlayerStats>();

        navAgent.destination = Player.position;
        canAttack = true;

        if (initialDestination != Vector3.zero)
            movement = WalkTo;
        else
        {
            if (navAgent.remainingDistance < 1)
            {
                if (meleeEnemy)
                    movement = MeleeMovement;
                else
                    movement = RangeMovement;
            }
        }
    }
    public void WalkTo()
    {
        navAgent.destination = initialDestination;

        if (navAgent.remainingDistance < 1)
        {
            if (meleeEnemy)
                movement = MeleeMovement;
            else
                movement = RangeMovement;
        }
    }
    void RangeMovement()
    {
        float playerDistance = Vector3.Distance(transform.position, Player.position);

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

            StartCoroutine(Attack());

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

            StartCoroutine(Attack());
            
        }
        else
        {
            Patrol();
        }
    }
    
    IEnumerator Attack()
    {
        if (!canAttack)
            yield break;

        canAttack = false;

        Collider[] hitPlayer = Physics.OverlapSphere(eattackPoint.position, attackRange, playerLayers);

        if (hitPlayer.Length > 0)
        {
            enemyScript.PlaySound(EnemySounds.Attack);
            playerHP.TakeDamage(attackDamage);
        }

        anim.SetTrigger("Attack");
        Debug.Log("attacked player");

        yield return new WaitForSeconds(attackDelay);

        canAttack = true;
    }
    void OnDrawGizmosSelected()
    {
        if (eattackPoint == null)
            return;

        Gizmos.DrawWireSphere(eattackPoint.position, attackRange);
    }

    private void Update()
    {
        if (UserInterfaceManager.frozen)
            return;

        anim.SetFloat("Speed", navAgent.velocity.magnitude);

        if (!enemyScript.stunned && !enemyScript.dead)
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

            navAgent.SetDestination(newPatrolPoint());
        }
            navAgent.isStopped = false;

        if (navAgent.remainingDistance < 2)
        {
            navAgent.SetDestination(newPatrolPoint());

        }
    }
}
