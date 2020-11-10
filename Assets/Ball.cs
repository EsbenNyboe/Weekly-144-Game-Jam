using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] float lifeDuration = 8;
    public Rigidbody rb;
    public bool hitEnemy;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    IEnumerator BallMade()
    {
        yield return new WaitForSeconds(0.3f);
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

        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerInteractions>().Stun();
            DestroyBall();
        }
    }
}
