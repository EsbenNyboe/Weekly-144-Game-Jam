using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    NavMeshAgent navAgent;
    public Transform Player;
    public float shootRange = 15;

    private void Awake()
    {
        navAgent = GetComponent<NavMeshAgent>();
        Player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    Coroutine stunCor;
    public bool inShootingRange()
    {
        return Vector3.Distance(Player.position, transform.position) < 15;
    }
    public void Stun(float duration = 1)
    {
        if (stunCor != null)
            StopCoroutine(stunCor);
        stunCor = StartCoroutine(_Stunned(duration));
        Debug.Log("enemy stunned");
    }

    IEnumerator _Stunned(float duration)
    {
        navAgent.isStopped = true;
        yield return new WaitForSeconds(duration);
        navAgent.isStopped = false;
    }
}
