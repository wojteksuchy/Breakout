using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    //what level the game is curently in
    //load and unload game levels
    //keep track of game states
    //generate other persistent systems


    private List<AsyncOperation> loadOperations;
    private string currentSceneName = string.Empty;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        loadOperations = new List<AsyncOperation>();
        Screen.SetResolution(540, 960, false);
        LoadScene("Game") ;
    }

    private void OnLoadSceneCompleted(AsyncOperation asyncOperation )
    {
        if (loadOperations.Contains(asyncOperation))
        {
            loadOperations.Remove(asyncOperation);
            //TODO: Transition between scenes
        }
        Debug.Log("Scene load completed");
    }

    private void OnUnloadSceneCompleted(AsyncOperation obj)
    {
        Debug.Log("Scene unload completed");
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

    

    public void UnloadScene(string sceneName) {
        AsyncOperation asyncOperation = SceneManager.UnloadSceneAsync(sceneName);
        if (asyncOperation == null)
        {
            Debug.LogError("[GameManager] Can't unload scene: " + sceneName);
            return;
        }
        asyncOperation.completed += OnUnloadSceneCompleted;
        currentSceneName = sceneName;
    }

    

    //private static GameManager instance;

    //public static GameManager Instance => instance;

    //private void Awake()
    //{
    //    if (instance != null)
    //    {
    //        Destroy(gameObject);
    //    }
    //    else
    //    {
    //        instance = this;
    //    }
    //}

    public bool IsGameStarted { get; set; }

    
}
