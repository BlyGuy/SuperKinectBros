using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    public static bool gameIsPaused = false;
    public GameObject PauseMenuUI;
    public GameObject pauseFirstButton;
    public GameObject ratBall;

    private void Start()
    {
        Resume();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown("Fire3"))
        {
            if (gameIsPaused)
                Resume();
            else
                Pause();
        }
    }

    public void Resume()
    {
        PauseMenuUI.SetActive(false);
        ratBall.SetActive(true);
        Time.timeScale = 1f;
        gameIsPaused = false;
        //Cursor.lockState = CursorLockMode.Confined;
    }

    public void Pause()
    {
        PauseMenuUI.SetActive(true);
        ratBall.SetActive(false);
        Time.timeScale = 0f;
        gameIsPaused = true;
        //Cursor.lockState = CursorLockMode.None;
        FindObjectOfType<MenuController>().SelectFirstMenuButton(pauseFirstButton);
    }
}
