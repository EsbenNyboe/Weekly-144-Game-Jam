using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundTriggerTest : MonoBehaviour
{
    public bool enableTest;

    private void Awake()
    {
        if (enableTest)
            Debug.Log("sound test enabled", gameObject);
    }

    private void Update()
    {
        if (enableTest)
        {
            //if (Input.GetKeyDown(KeyCode.Mouse0))
            //    AudioSystem.sb.playerServe.PlayDefault();
            //if (Input.GetKeyDown(KeyCode.Mouse1))
            //    AudioSystem.sb.playerSwing.PlayDefault();

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
