using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private MainMenu mainMenu;
    [SerializeField] private Camera dummyCamera;

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

    public void SetDummyCameraActive(bool active)
    {
        dummyCamera.gameObject.SetActive(active);
    }
}
