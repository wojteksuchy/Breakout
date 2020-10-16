using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickManager : MonoBehaviour
{
    private static BrickManager instance;

    public static BrickManager Instance => instance;

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

    public Sprite[] Sprites;
    private int maxRow = 17;
    private int maxColumns = 12;

    public List<int[,]> LevelData { get; set; }

    private void Start()
    {
        LevelData = LoadLevelData();
    }

    private List<int[,]> LoadLevelData()
    {
        TextAsset text = Resources.Load("levels") as TextAsset;
        string[] rows = text.text.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
        List<int[,]> levelData = new List<int[,]>();
        int[,] currentLevel = new int[maxRow, maxColumns];
        int currentRow = 0;

        for (int row = 0; row < rows.Length; row++)
        {
            string line = rows[row];
            if (line.IndexOf("--") == -1)
            {
                string[] bricks = line.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                for (int column = 0; column < bricks.Length; column++)
                {
                    currentLevel[currentRow, column] = int.Parse(bricks[column]);
                }
                currentRow++;
            }
            else
            {//end level
                currentRow = 0;
                levelData.Add(currentLevel);
                currentLevel = new int[maxRow, maxColumns];
            }
        }
        //int.TryParse(bricks[col], out currentLevel[currentRow, col]);
        return levelData;
    }
    
}
