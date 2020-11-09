using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    NavMeshAgent navAgent;
    private void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
    }

    Coroutine stunCor;
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
