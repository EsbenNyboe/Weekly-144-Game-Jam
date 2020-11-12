using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayersManager : MonoBehaviour
{
    public int Player, Enemy, Default;

    public static int PlayerBall, EnemyBall, DefaultBall;

    private void Start()
    {
        Default = 13;

        PlayerBall = Player;
        EnemyBall = Enemy;
        DefaultBall = Default;
    }
}
