using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player_Racket : MonoBehaviour
{
    public float swingDuration = 1;
    Collider col;
    public Transform player;
    public float swingForce = 10;
    public Transform ballSpawnPoint;
    BallMaker_Player ballPool;

    public int maxBalls = 5;
    int currentBalls;
    public float rechargeTime;
    float timer;

    public TextMeshProUGUI ballCount;

    private void Start()
    {
        currentBalls = maxBalls;
        ballCount.text = currentBalls.ToString();

        col = GetComponent<Collider>();
        ballPool = GetComponent<BallMaker_Player>();
        if (ballSpawnPoint == null)
            ballSpawnPoint = transform;

        player = transform.parent;
    }

    private void Update()
    {
        if (UserInterfaceManager.frozen)
            return;

        if (currentBalls < maxBalls)
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
            } else
            {
                timer = rechargeTime;
                currentBalls++;
                ballCount.text = currentBalls.ToString();
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            Swing();
        }
        if (Input.GetMouseButtonDown(1))
        {
            Shoot();
        }
    }

    public Animator racketAnimator;
    void Swing()
    {
        racketAnimator.SetTrigger("Swing");
        AudioSystem.sb.playerSwing.PlayDefault();
        StartCoroutine(_Swing());
    }
    public float throwForce = 20;

    Dictionary<int, Rigidbody> RBs = new Dictionary<int, Rigidbody>();

    void GetBall()
    {
        currentBalls--;
        ballCount.text = currentBalls.ToString();
        timer = rechargeTime;
    }

    void Shoot()
    {
        if (currentBalls <= 0)
            return;

        AudioSystem.sb.playerServe.PlayDefault();
        col.enabled = false;
        var ball = ballPool.GetObject();
        ball.transform.position = ballSpawnPoint.position;
        ball.transform.parent = transform;

        GetBall();

        Vector3 dir = (transform.up + ball.transform.position) - ball.transform.position;
        Rigidbody rb;



        if (!RBs.TryGetValue(ball.GetHashCode(), out rb))
        {
            rb = ball.GetComponent<Rigidbody>();
            RBs.Add(ball.GetHashCode(), rb);
        }
        rb.gameObject.layer = LayersManager.PlayerBall;

        rb.AddForce((dir) * throwForce, ForceMode.Force);
    }

    IEnumerator _Swing()
    {
        col.enabled = true;
        yield return new WaitForSeconds(swingDuration);
        col.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ball"))
        {
            AudioSystem.sb.playerShoot.PlayDefault();
            Rigidbody ballRb;

            if (!RBs.TryGetValue(other.GetHashCode(), out ballRb))
            {
                ballRb = other.GetComponent<Rigidbody>();
                RBs.Add(other.GetHashCode(), ballRb);
            }

            ballRb.velocity = Vector3.zero;
            Vector3 dir = (player.forward + ballRb.transform.position) - ballRb.transform.position;
            ballRb.AddForce((dir) * swingForce, ForceMode.Impulse);
            ballRb.gameObject.layer = LayersManager.PlayerBall;

        }
    }
}
