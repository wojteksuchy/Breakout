using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour
{
    public delegate void OnBrickDestruction(Brick brick);
    public static event OnBrickDestruction BrickDestroyed;
    //public static event Action<Brick> BrickDestroyed;

    public int HitPoint = 1;


    private void OnCollisionEnter2D(Collision2D collision)
    {
        Ball ball = collision.gameObject.GetComponent<Ball>();
        CollisionLogic(ball);
    }

    private void CollisionLogic(Ball ball)
    {
        HitPoint--;
        if (HitPoint <=0)
        {
            BrickDestroyed?.Invoke(this);
            //TODO: Add destroy effect
            Destroy(gameObject);
        }
        else
        {
            //TODO: Change visual
        }
    }
}
