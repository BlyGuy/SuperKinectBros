using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    public static bool gameIsPaused = false;
    public GameObject PauseMenuUI;
    public GameObject BodyMenuController;

    private float pressDownTime = 0.0f;
    private bool pauseIntent = false;

    private void Start()
    {
        Resume();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Return))
        {
            //Add to the pauseTimer
            pressDownTime += Time.deltaTime;
            //Perform a pause after 1 second or a immediate unpause
            if (pauseIntent)
            {
                if (!gameIsPaused && pressDownTime > 1.0f)
                {
                    Pause();
                    pressDownTime = 0.0f;
                    pauseIntent = false;
                }
                else if (gameIsPaused)
                {
                    Resume();
                    pressDownTime = 0.0f;
                    pauseIntent = false;
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyUp(KeyCode.Return))
        {
            //Start the pause-timer and indicate that the user wants to pause
            pressDownTime = 0.0f;
            pauseIntent = true;
        }
    }

    public void Resume()
    {
        PauseMenuUI.SetActive(false);
        BodyMenuController.SetActive(false);
        //Time.timeScale = 1f;
        gameIsPaused = false;
    }

    public void Pause()
    {
        PauseMenuUI.SetActive(true);
        BodyMenuController.SetActive(true);
        //Time.timeScale = 0f;
        gameIsPaused = true;
    }
}
