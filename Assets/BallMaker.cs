using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMaker : GenericObjectPool<Ball>
{
    public float throwForce = 10;

    public static BallMaker Instance;

    private void Start()
    {
        //StartCoroutine(_MakeBall());
        Instance = this;
    }

    public override void AddObject()
    {
        base.AddObject();
    }

    IEnumerator _MakeBall()
    {
        Ball ball = GetObject();
        ball.transform.position = transform.position;

        ball.GetComponent<Rigidbody>().AddForce(throwForce*transform.forward, ForceMode.Impulse);
        yield return new WaitForSeconds(2);
        StartCoroutine(_MakeBall());

    }
}
