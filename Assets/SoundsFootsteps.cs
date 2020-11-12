using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundsFootsteps : MonoBehaviour
{
    public EnemySoundbank enemySoundbank;

    PlayerStats playerHP;

    EnemyShooter shootScript;
    EnemyMovement moveScript;

    private void Awake()
    {
        playerHP = FindObjectOfType<PlayerStats>();
        transform.parent.gameObject.TryGetComponent(out shootScript);
        transform.parent.gameObject.TryGetComponent(out moveScript);
    }

    public void PlaySoundFootsteps()
    {
        enemySoundbank.enemyFootsteps.PlayDefault();
    }
    public void PlaySoundAttack()
    {
        enemySoundbank.enemyAttack.PlayDefault();
        AudioSystem.sb.playerDamaged.PlayDefault();
        playerHP.TakeDamage(moveScript.attackDamage);
    }

    public void PlaySoundShoot()
    {
        shootScript.Shoot();
    }
}
