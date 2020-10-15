using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : MonoBehaviour
{
    private Camera mainCamera;
    private SpriteRenderer spriteRenderer;
    private float paddleInitPositionY;
    private float defaultPaddleWidth = 192;
    private float defaulthLeftBorder = 135;
    private float defaulthRightBorder = 405;

    private static Paddle instance;

    public static Paddle Instance => instance;

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
        mainCamera = Camera.main;
        spriteRenderer = GetComponent<SpriteRenderer>();
        paddleInitPositionY = transform.position.y;
    }
    

    private void Start()
    {
    
    }

    private void Update()
    {
        PaddleMovement();
    }

    private void PaddleMovement()
    {
        
        float mousePositionWorldX = mainCamera.ScreenToWorldPoint(new Vector3(ClampPaddlePosition(), 0, 0)).x;
        transform.position = new Vector3(mousePositionWorldX, paddleInitPositionY, 0);
    }

    private float ClampPaddlePosition()
    {
        float mausePositions;
        float leftBorder = defaulthLeftBorder - PaddleShift();
        float rightBorder = defaulthRightBorder + PaddleShift();
        return mausePositions = Mathf.Clamp(Input.mousePosition.x, leftBorder, rightBorder);
    }

    private float PaddleShift()
    {
        float paddleShift;
        paddleShift = (defaultPaddleWidth - ((defaultPaddleWidth / 2) * spriteRenderer.size.x)) / 2;
        return paddleShift;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            Rigidbody2D ballRB = collision.gameObject.GetComponent<Rigidbody2D>();
            float ballSpeed = BallManager.Instance.BallSpeed;
            Vector3 hitPoint = collision.contacts[0].point;
            Vector3 paddleCenter = new Vector3(gameObject.transform.position.x, transform.position.y);
            ballRB.velocity = Vector2.zero;
            float difference = paddleCenter.x - hitPoint.x;
            if (hitPoint.x < paddleCenter.x )
            {
                //left side of paddle
                ballRB.AddForce(new Vector2(-Mathf.Abs(difference * 200), ballSpeed));
            }
            else
            {
                ballRB.AddForce(new Vector2(Mathf.Abs(difference * 200), ballSpeed));
            }
        }
    }
}
