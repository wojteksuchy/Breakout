using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class Brick : MonoBehaviour
{
    //public delegate Brick OnBrickDestruction(Brick brick);
    //public static event OnBrickDestruction BrickDestroyed;
    public static event Action<Brick> BrickDestroyed;

    private SpriteRenderer spriteRenderer;

    public ParticleSystem DystroyEffect;
    public int HitPoints = 1;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void Start()
    {
        spriteRenderer.sprite = BrickManager.Instance.Sprites[HitPoints - 1]; // delegat ??
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Ball ball = collision.gameObject.GetComponent<Ball>();
        CollisionLogic(ball);
    }

    private void CollisionLogic(Ball ball)
    {
        HitPoints--;
        if (HitPoints <=0)
        {
            BrickDestroyed?.Invoke(this);
            BrickDestroyEffect();
            Destroy(gameObject);
        }
        else
        {
            spriteRenderer.sprite = BrickManager.Instance.Sprites[HitPoints - 1];
        }
    }

    private void BrickDestroyEffect()
    {
        Vector3 brickPosition = gameObject.transform.position;
        Vector3 spawnPosition = new Vector3(brickPosition.x, brickPosition.y, brickPosition.z - 0.2f);
        GameObject effect = Instantiate(DystroyEffect.gameObject, spawnPosition, Quaternion.identity);

        MainModule mainModule = effect.GetComponent<ParticleSystem>().main;
        mainModule.startColor = AverageColorFromTexture(spriteRenderer.sprite.texture);
        Destroy(effect, mainModule.startLifetime.constant);
    }

    private Color AverageColorFromTexture(Texture2D tex)
    {

        Color32[] texColors = tex.GetPixels32();

        int total = texColors.Length;

        float r = 0;
        float g = 0;
        float b = 0;

        for (int i = 0; i < total; i++)
        {

            r += texColors[i].r;

            g += texColors[i].g;

            b += texColors[i].b;

        }

        
        Color color = new Color32((byte)(r / total), (byte)(g / total), (byte)(b / total), 0);
        color.a = 1;
        
        return color;

    }

    internal void Init(Transform continerTransform, Sprite sprite, Color color, int hitPoints)
    {
        transform.SetParent(continerTransform);
        spriteRenderer.sprite = sprite;
        spriteRenderer.color = color;
        HitPoints = hitPoints;

    }
}
