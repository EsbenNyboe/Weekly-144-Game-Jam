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
    public int maxHealth = 100;
    int currentHealth;
    public bool stunned;
    public bool dead;

    public ParticleSystem stunEffect;
    public Animator anim;

    private void Awake()
    {
        navAgent = GetComponent<NavMeshAgent>();
        Player = GameObject.FindGameObjectWithTag("Player").transform;
        playerScript = Player.GetComponent<PlayerInteractions>();
        currentHealth = maxHealth;
        stunned = false;
        dead = false;
    }

    public void TakeDamage(int damage)
    {
        anim.SetTrigger("Hit");
        anim.SetInteger("HitInt", Random.Range(0, 2));
        currentHealth -= damage;

        //Play hurt animation
        Debug.Log("Enemy damaged!");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Enemy Down!");
        dead = true;
        //Call death animation
        anim.SetTrigger("Dead");
        //Disable the enemy

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
        //navAgent.isStopped = true;
        stunEffect.Play();
        stunned = true;
        navAgent.destination = transform.position;
        anim.SetBool("Stun", true);
        yield return new WaitForSeconds(duration);
        anim.SetBool("Stun", false);
        stunEffect.Stop();
        stunEffect.Clear();
        stunned = false;

        //navAgent.isStopped = false;
    }
}
