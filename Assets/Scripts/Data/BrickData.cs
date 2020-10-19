using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "newBrickData", menuName = "Data/Brick Data/ Base Data")]
public class BrickData : ScriptableObject
{
    
    public Sprite[] Sprites;

    public int HitPoints;

    public Color AverageColorFromTexture(Texture2D tex)
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


}


