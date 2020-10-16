using System;
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

    [SerializeField] private int CurrentLevel;

    private int maxRows = 17;
    private int maxColumns = 12;
    private float initialBrickSpawnPositionX = -1.96f;
    private float initialBrickSpawnPositionY = 3.325f;

    private GameObject bricksContainer;

    public List<int[,]> LevelData { get; set; }
    [SerializeField] private List<BrickData> Bricks;
    public Sprite[] Sprites;
    public Brick brickPrefab;
    public Color[] BrickColors;

    private float shiftAmount = 0.3f;

    public List<Brick> RemaningBricks { get; set; }
    public int InitialBrickCount { get; private set; }

    private void Start()
    {
        
        bricksContainer = new GameObject("Brick Container");
        RemaningBricks = new List<Brick>();
        LevelData = LoadLevelData();
        GenerateBricks();


    }


    private List<int[,]> LoadLevelData()
    {
        TextAsset text = Resources.Load("levels") as TextAsset;
        string[] rows = text.text.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
        List<int[,]> levelData = new List<int[,]>();
        int[,] currentLevel = new int[maxRows, maxColumns];
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
                currentLevel = new int[maxRows, maxColumns];
            }
        }
        //int.TryParse(bricks[col], out currentLevel[currentRow, col]);
        return levelData;
    }


    private void GenerateBricks()
    {
        int[,] currentLevelData = LevelData[CurrentLevel];
        float currentSpawnX = initialBrickSpawnPositionX;
        float currentSpawnY = initialBrickSpawnPositionY;
        float zShift = 0;
        for (int row = 0; row < maxRows; row++)
        {
            for (int column = 0; column < maxColumns; column++)
            {
                int brickType = currentLevelData[row, column];

                if (brickType > 0)
                {
                    Brick newBrick = Instantiate(brickPrefab, new Vector3(currentSpawnX, currentSpawnY, 0.0f - zShift), Quaternion.identity) as Brick;
                    newBrick.Init(bricksContainer.transform, Bricks[brickType - 1].Sprites[brickType - 1], BrickColors[brickType], brickType);

                    RemaningBricks.Add(newBrick);
                    zShift += 0.0001f;
                }
                currentSpawnX += shiftAmount;
                if (column + 1 == maxColumns)
                {
                    currentSpawnX = initialBrickSpawnPositionX;
                }
            }
            currentSpawnY -= shiftAmount;
        }
        InitialBrickCount = RemaningBricks.Count; 
    }

}
