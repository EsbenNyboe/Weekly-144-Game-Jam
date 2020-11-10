using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMaker : GenericObjectPool<Ball>
{
    public float throwForce = 10;

    public static BallMaker Instance;

    private void Start()
    {
        Instance = this;
    }

    public override void AddObject()
    {
        base.AddObject();
    }

    public override void ReturnObject(Ball DoneObject)
    {
        base.ReturnObject(DoneObject);
        DoneObject.rb.Sleep();
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
