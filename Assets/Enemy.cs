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

    EnemySoundbank enemySoundbank;

    public ParticleSystem blood;
    //AudioSource audio;
    //public AudioClip attacking, takeDamage, spawned, inRange, stun, died;



    private void Awake()
    {
        enemySoundbank = GetComponentInChildren<EnemySoundbank>();
        navAgent = GetComponent<NavMeshAgent>();
        //audio = GetComponent<AudioSource>();
        Player = GameObject.FindGameObjectWithTag("Player").transform;
        playerScript = FindObjectOfType<PlayerInteractions>();
        currentHealth = maxHealth;
        stunned = false;
        dead = false;
    }
    private void Start()
    {
        PlaySound(EnemySounds.Spawned);
    }

    public void TakeDamage(int damage)
    {
        AudioSystem.sb.playerAttackDamage.PlayDefault();
        PlaySound(EnemySounds.GetHit);
        blood.Play();
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

        
        dead = true;
        //Call death animation
        anim.SetBool("Dead", true);
        //Disable the enemy
        PlaySound(EnemySounds.Dead);
        navAgent.isStopped = true;
        GetComponent<Collider>().enabled = false;
        WaveSpawner.instance.EnemyKilled();
    }

    Coroutine stunCor;
    public bool inShootingRange()
    {
        return Vector3.Distance(Player.position, transform.position) < shootRange && !playerScript.stun;
    }
    public bool inShootingRangeNoCareAboutStun()
    {
        return Vector3.Distance(Player.position, transform.position) < shootRange;
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
        PlaySound(EnemySounds.Stunned);
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

    public void PlaySound(EnemySounds sound)
    {
        switch (sound)
        {
            case EnemySounds.Attack:
                //enemySoundbank.enemyAttack.PlayDefault();
                break;
            case EnemySounds.GetHit:
                enemySoundbank.enemyDamaged.PlayDefault();
                break;
            case EnemySounds.InRange:
                enemySoundbank.enemyInRangeBrute.PlayDefault(); // fix - this shouldn't happen on shooters, i think
                break;
            case EnemySounds.Spawned:
                enemySoundbank.enemySpawn.PlayDefault();
                break;
            case EnemySounds.Stunned:
                enemySoundbank.enemyStunned.PlayDefault();
                break;
            case EnemySounds.Dead:
                enemySoundbank.enemyKilled.PlayDefault();
                break;
            default:
                break;
        }
    }

    
}
