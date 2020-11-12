using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundsFootsteps : MonoBehaviour
{
    public EnemySoundbank enemySoundbank;

    public void PlaySoundFootsteps()
    {
        enemySoundbank.enemyFootsteps.PlayDefault();
    }
}
