using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyShooter : MonoBehaviour
{
    Enemy enemyScript;
    public Transform shootPosition;

    public float shootDelay = 3;
    float nextShootTime;

    bool canShoot;
    public float shootForce = 10;

    private void Start()
    {
        enemyScript = GetComponent<Enemy>();        
    }

    private void Update()
    {
        canShoot = enemyScript.inShootingRange();

        if (canShoot)
        {
            if (nextShootTime < shootDelay)
            {
                nextShootTime += Time.deltaTime;
            }
            else
            {
                Shoot();
            }
        }
    }

    void Shoot()
    {
        Vector3 dir = (enemyScript.Player.position+(transform.up*2)) - shootPosition.position;
        Ball newBall = BallMaker.Instance.GetObject();
        newBall.transform.position = shootPosition.position;
        newBall.rb.AddForce((dir.normalized) * shootForce,ForceMode.Impulse);
        nextShootTime = 0;

    }
}
