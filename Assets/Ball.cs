using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] float lifeDuration = 8;
    IEnumerator BallMade()
    {
        yield return new WaitForSeconds(0.2f);
        if (transform.parent != null)
            transform.parent = null;

        Invoke("DestroyBall", lifeDuration);
    }

    void DestroyBall()
    {
        BallMaker.Instance.ReturnObject(this);
    }

    private void OnEnable()
    {
        StartCoroutine(BallMade());
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<Enemy>().Stun();
            DestroyBall();
        }
    }
}
