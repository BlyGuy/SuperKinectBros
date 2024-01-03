using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class TutorialManager : MonoBehaviour
{
    enum ETutorialState
    {
        STATE_RUN_RIGHT,
        STATE_RUN_LEFT,
        STATE_JUMP,
        STATE_FIREBALL,
        STATE_PAUSE,
        STATE_END
    }

    private ETutorialState state = ETutorialState.STATE_RUN_RIGHT;
    private KeyCode[] gestureKeyCodes = 
    { 
        KeyCode.D,
        KeyCode.A,
        KeyCode.H,
        KeyCode.J,
        KeyCode.Return,
        KeyCode.Escape //End
    };
    //private string[] gestureStrings =
    //{
    //    "Run to the right",
    //    "Run to the left",
    //    "Jump using your arms",
    //    "Gooi je arm naar de zijkant",
    //    "Pauze door met je handen een kruisje te maken",
    //    "You did it!!!",
    //};
    private int[] gesturePerformances = {0, 0, 0, 0, 0, 0};
    public VideoClip[] gestureClips;
    public VideoPlayer gesturePlayer;

    private void Start()
    {
        gesturePlayer.clip = gestureClips[0];
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(gestureKeyCodes[(int)state]))
        {
            //TODO: Meer fanfare of feedback-mario reageert
            gesturePerformances[(int)state]++;
        }

        //After five performances, go to next state
        if (gesturePerformances[(int)state] >= 5)
        {
            state++;
            gesturePlayer.clip = gestureClips[(int)state];
        }

        //Check if the tutorial has ended
        if (state == ETutorialState.STATE_END)
        {
            //TODO: Meer fanfare
            SceneManager.LoadScene(0);
        }
    }
}
