using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{

    //what level the game is curently in
    //load and unload game levels
    //keep track of game states
    //generate other persistent systems
    public Events.EventGameState OnGameStateChanged;

    public enum GameState
    {
        Pregame,
        Running,
        Paused
    }

    private GameState currentGameState = GameState.Pregame;

    public GameState GetCurrentGameState
    {
        get { return currentGameState; }
        private set { currentGameState = value; }
    }

    public GameObject[] SystemPrefabs; // Array of system prefabs to be created

    private List<GameObject> instancedSystemPrefabs; // List of created system prefabs
    private List<AsyncOperation> loadOperations;

    private string currentSceneName = string.Empty;


    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        instancedSystemPrefabs = new List<GameObject>();
        loadOperations = new List<AsyncOperation>();
        Screen.SetResolution(540, 960, false);
        InstantiateSystemPrefabs();
        UIManager.Instance.OnMainMenuFadeComplete.AddListener(HandleMainMenuFadeComplete);
        //LoadScene("Game");
    }

    private void Update()
    {
        if (currentGameState == GameManager.GameState.Pregame)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    private void InstantiateSystemPrefabs()
    {
        GameObject prefabInstance;

        for (int i = 0; i < SystemPrefabs.Length; i++)
        {
            prefabInstance = Instantiate(SystemPrefabs[i]);
            instancedSystemPrefabs.Add(prefabInstance);
        }
    }

    private void UpdateState(GameState state)
    {
        GameState previousGameState = currentGameState;
        currentGameState = state;

        switch (currentGameState)
        {
            case GameState.Pregame:
                Time.timeScale = 1.0f;
                break;
            case GameState.Running:
                Time.timeScale = 1.0f;
                break;
            case GameState.Paused:
                Time.timeScale = 0.0f;
                break;
            default:
                break;
        }

        OnGameStateChanged?.Invoke(currentGameState, previousGameState);
    }

    private void OnLoadSceneCompleted(AsyncOperation asyncOperation)
    {
        if (loadOperations.Contains(asyncOperation))
        {
            loadOperations.Remove(asyncOperation);
            if (loadOperations.Count == 0)
            {
                UpdateState(GameState.Running);
            }

        }
        Debug.Log("Scene load completed");
    }

    private void OnUnloadSceneCompleted(AsyncOperation obj)
    {
        Debug.Log("Scene unload completed");
    }

    private void HandleMainMenuFadeComplete(bool fadeOut)
    {
        if (!fadeOut)
        {
            UnloadScene(currentSceneName);
        }
    }

    public void LoadScene(string sceneName)
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        if (asyncOperation == null)
        {
            Debug.LogError("[GameManager] Can't load scene: " + sceneName);
            return;
        }
        asyncOperation.completed += OnLoadSceneCompleted;
        loadOperations.Add(asyncOperation);
        currentSceneName = sceneName;
    }

    public void UnloadScene(string sceneName)
    {
        AsyncOperation asyncOperation = SceneManager.UnloadSceneAsync(sceneName);
        if (asyncOperation == null)
        {
            Debug.LogError("[GameManager] Can't unload scene: " + sceneName);
            return;
        }
        asyncOperation.completed += OnUnloadSceneCompleted;
        currentSceneName = sceneName;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        for (int i = 0; i < instancedSystemPrefabs.Count; i++)
        {
            Destroy(instancedSystemPrefabs[i]);

        }
        instancedSystemPrefabs.Clear();
    }
    public void StartGame()
    {
        LoadScene("Game");
    }

    public void TogglePause()
    {
        UpdateState(currentGameState == GameState.Running ? GameState.Paused : GameState.Running);
    }

    public void RestartGame()
    {
        UpdateState(GameState.Pregame);
    }

    public void QuitGame()
    {
        //TODO ask 
        Application.Quit();
    }

    public bool IsGameStarted { get; set; }


}
