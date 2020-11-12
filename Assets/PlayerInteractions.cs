using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteractions : MonoBehaviour
{
    public bool stun;
    Rigidbody rb;

    public TextMeshProUGUI stunUI;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    Coroutine stunCor;
    public void Stun(float duration = 1)
    {
        if (stunCor != null)
            StopCoroutine(stunCor);
        stunCor = StartCoroutine(_Stunned(duration));
    }

    IEnumerator _Stunned(float duration)
    {
        rb.isKinematic = stun = stunUI.enabled = true;
        Soundbank.instance.playerStunned.PlayDefault();

        yield return new WaitForSeconds(duration);
        rb.isKinematic = stun = stunUI.enabled = false;
    }
}
