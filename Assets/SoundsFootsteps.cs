using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundsFootsteps : MonoBehaviour
{
    public EnemySoundbank enemySoundbank;

    PlayerStats playerHP;

    private void Awake()
    {
        playerHP = FindObjectOfType<PlayerStats>();
    }

    public void PlaySoundFootsteps()
    {
        enemySoundbank.enemyFootsteps.PlayDefault();
    }
    public void PlaySoundSwing()
    {
        enemySoundbank.enemySwing.PlayDefault();
    }
    public void PlaySoundSwingVoice()
    {
        enemySoundbank.enemySwingVoice.PlayDefault();
    }
    public void PlaySoundAttack()
    {
        enemySoundbank.enemyAttack.PlayDefault();
        AudioSystem.sb.playerDamaged.PlayDefault();
        playerHP.TakeDamage(EnemyMovement.attackDamage);
    }
}
