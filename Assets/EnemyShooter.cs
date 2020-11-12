using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyShooter : MonoBehaviour
{
    Enemy enemyScript;
    public Transform shootPosition;
    Transform shootTarget;

    public float shootDelay = 3;
    float nextShootTime;

    bool canShoot;
    public float shootForce = 10;

    private void Start()
    {
        enemySoundbank = GetComponentInChildren<EnemySoundbank>();
        enemyScript = GetComponent<Enemy>();
        shootTarget = GameObject.FindGameObjectWithTag("PlayerHead").transform;
    }

    private void Update()
    {
        if (UserInterfaceManager.frozen || enemyScript.dead || enemyScript.stunned)
            return;

        canShoot = enemyScript.inShootingRange();

        if (canShoot)
        {
            if (nextShootTime < shootDelay)
            {
                nextShootTime += Time.deltaTime;
            }
            else
            {

                PlayShootAnimation();
            }
        }
    }

    EnemySoundbank enemySoundbank;

    void PlayShootAnimation()
    {
        enemyScript.anim.SetTrigger("Shoot");

    }
    public void Shoot()
    {
        enemySoundbank.enemyShoot.PlayDefault();

        Vector3 dir = (shootTarget.position+(transform.up*2)) - shootPosition.position;
        Ball newBall = BallMaker.Instance.GetObject();
        newBall.gameObject.layer = LayersManager.EnemyBall;


        newBall.transform.position = shootPosition.position;
        newBall.rb.AddForce((dir.normalized) * shootForce,ForceMode.Impulse);
        nextShootTime = 0;

    }
}
