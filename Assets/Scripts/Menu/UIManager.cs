using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private MainMenu mainMenu;
    [SerializeField] private Camera dummyCamera;
    [SerializeField] private PauseMenu pauseMenu;

    public Events.EventFadeComplete OnMainMenuFadeComplete;
    private void Start()
    {
        mainMenu.OnMainMenuFadeComplete.AddListener(HandleMainMenuComplete);
        GameManager.Instance.OnGameStateChanged.AddListener(HandleGameStateChanged);
    }

    

    private void Update()
    {
        if (GameManager.Instance.GetCurrentGameState != GameManager.GameState.Pregame)
        {
            return;
        }
        if (Input.GetMouseButtonDown(0))
        {
            //mainMenu.FadeOut();
            GameManager.Instance.StartGame();
        }
    }
    private void HandleMainMenuComplete(bool fadeOut)
    {
        OnMainMenuFadeComplete?.Invoke(fadeOut);
    }

    private void HandleGameStateChanged(GameManager.GameState currentGameState, GameManager.GameState previousGameState)
    {
        pauseMenu.gameObject.SetActive(currentGameState == GameManager.GameState.Paused);
    }

    public void SetDummyCameraActive(bool active)
    {
        dummyCamera.gameObject.SetActive(active);
    }
}
