using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundTriggerTest : MonoBehaviour
{
    public bool enableTest;

    private void Update()
    {
        if (enableTest)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                AudioSystem.sb.racketSwing.PlayDefault();
                Debug.Log("play test", gameObject);
            }
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                AudioSystem.sb.playerAttackDamage.VolumeFade(1, 3, 2);
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                AudioSystem.sb.playerAttackDamage.VolumeFade(0, 0.5f);
            }
        }
    }
}
