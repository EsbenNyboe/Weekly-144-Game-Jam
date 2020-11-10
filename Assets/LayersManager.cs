using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayersManager : MonoBehaviour
{
    public int Player, Enemy;

    public static int PlayerBall, EnemyBall;

    private void Start()
    {
        PlayerBall = Player;
        EnemyBall = Enemy;
    }
}
