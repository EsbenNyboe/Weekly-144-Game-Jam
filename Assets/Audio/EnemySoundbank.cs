using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemySoundbank : MonoBehaviour
{
    public SoundObject enemyServe;
    public SoundObject enemySwing;
    public SoundObject enemyShoot;
    public SoundObject enemyAttack;
    public SoundObject enemyDamaged; // extra
    public SoundObject enemyKilled; // extra
    public SoundObject enemyStunned;
    public SoundObject enemyFootsteps;
    public SoundObject enemyInRangeBrute;
    public SoundObject enemyInRangeShooter;
    public SoundObject enemySpawn;

    public static EnemySoundbank instance;

    private void Awake()
    {
        instance = this;
    }
}
