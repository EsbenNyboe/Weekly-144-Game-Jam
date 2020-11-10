using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    NavMeshAgent navAgent;
    public Transform Player;
    [HideInInspector] public PlayerInteractions playerScript;
    public float shootRange = 15;
    public ParticleSystem stunEffect;

    private void Awake()
    {
        navAgent = GetComponent<NavMeshAgent>();
        Player = GameObject.FindGameObjectWithTag("Player").transform;
        playerScript = Player.GetComponent<PlayerInteractions>();
    }

    Coroutine stunCor;
    public bool inShootingRange()
    {
        return Vector3.Distance(Player.position, transform.position) < shootRange && !playerScript.stun;
    }
    public void Stun(float duration = 1)
    {
        if (stunCor != null)
            StopCoroutine(stunCor);
        stunCor = StartCoroutine(_Stunned(duration));
    }

    IEnumerator _Stunned(float duration)
    {
        navAgent.isStopped = true;
        stunEffect.Play();
        yield return new WaitForSeconds(duration);
        stunEffect.Stop();
        stunEffect.Clear();
        navAgent.isStopped = false;
    }
}
