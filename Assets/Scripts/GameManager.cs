using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{

    //what level the game is curently in
    //load and unload game levels
    //keep track of game states
    //generate other persistent systems
    public GameObject[] SystemPrefabs; // Array of system prefabs to be created

    private List<GameObject> instancedSystemPrefabs; // List of created system prefabs
    private List<AsyncOperation> loadOperations;

    private string currentSceneName = string.Empty;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        loadOperations = new List<AsyncOperation>();
        Screen.SetResolution(540, 960, false);
        LoadScene("Game");
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

    

    private void OnLoadSceneCompleted(AsyncOperation asyncOperation)
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

    public bool IsGameStarted { get; set; }


}
