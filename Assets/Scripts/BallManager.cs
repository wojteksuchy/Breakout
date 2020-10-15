using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallManager : MonoBehaviour
{

    [SerializeField] Ball ballPrefab;
    
    private float yOffset = .27f;
    private Ball ball;
    private Rigidbody2D ballRB;
    
    public List<Ball> Balls { get;private set; }
    public float BallSpeed = 250f;

    private static BallManager instance;

    public static BallManager Instance => instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }


    private void Start()
    {
        InitBall();
    }

    private void Update()
    {
        //if game is not started make ball position at paddle
        if (!GameManager.Instance.IsGameStarted)
        {
            Vector3 paddlePosition = Paddle.Instance.gameObject.transform.position;
            Vector3 ballPosition = new Vector3(paddlePosition.x, paddlePosition.y + yOffset, 0);
            ball.transform.position = ballPosition;
            if (Input.GetMouseButtonDown(0))
            {
                ballRB.isKinematic = false;
                ballRB.AddForce(new Vector2(0, BallSpeed));
                GameManager.Instance.IsGameStarted = true;
            }
        }
        
    }

    private void InitBall()
    {
        Vector3 paddlePosition = Paddle.Instance.gameObject.transform.position;
        Vector3 startingPosition = new Vector3(paddlePosition.x, paddlePosition.y + yOffset,0);
        ball = Instantiate(ballPrefab, startingPosition, Quaternion.identity);
        ballRB = ball.GetComponent<Rigidbody2D>();

        Balls = new List<Ball>
        {
            ball
        };
    }
}
