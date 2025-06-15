using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameMenuUI : MonoBehaviour
{
    private PlayerActionsInput inputActions;
    public GameObject menuUIPanel;
    public GameObject gameUIPanel;

    private void Awake()
    {
        inputActions = new PlayerActionsInput();
        inputActions.Enable();
    }

    private void Update()
    {
        if (inputActions == null)
        {
            Debug.LogWarning("INPUT ACTIONS IS NULL");
        }

        if (menuUIPanel == null)
        {
            Debug.LogWarning("IT IS GONEE");
        }

        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            ShowMenu();
        }

        if (Time.timeScale == 0)
        {
            Debug.LogWarning("GAME IS PAUSED");
        }
    }
    public void Resume()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1f;    //  MAKE SURE THE GAME IS NOT PAUSED
    }
    private void ShowMenu()
    {
        if (menuUIPanel.activeInHierarchy)
        {
            menuUIPanel.SetActive(false);
            gameUIPanel.SetActive(true);

            Time.timeScale = 1;
            Cursor.lockState = CursorLockMode.Locked;

        }
        else
        {
            menuUIPanel.SetActive(true);
            gameUIPanel.SetActive(false);

            Time.timeScale = 0;
            
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
