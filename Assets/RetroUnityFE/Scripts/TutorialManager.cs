using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    private string[] gestureStrings =
    {
        "Run right using your right knee.\nRaise it to the right to keep running.",
        "Run left using your left knee.\nHold your knee up to keep running.",
        "Jump using your arms.\nIt is recommended to fully put your arm down before the next jump.",
        "Throw fireballs (and run faster) by holding one of your arms out.\nTo throw another fireball you must retreat your arm first.",
        "Pause by crossing your arms above your shoulders. This gesture also needs to be performed to start the game. To go into the pause menu from the main game, hold this gesture for 1 second.",
        "You did it!!! :D",
    };
    private int[] gesturePerformances = {0, 0, 0, 0, 0, 0};
    public VideoClip[] gestureClips;
    public VideoPlayer gesturePlayer;
    public Animator animator;
    public Animator animator_vuurbal;
    public TMP_Text tutorText;

    private void Start()
    {
        gesturePlayer.clip = gestureClips[0];
        tutorText.text = gestureStrings[0];
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(gestureKeyCodes[(int)state]))
        {
            gesturePerformances[(int)state]++;
        }
        
        if (Input.GetKeyDown(gestureKeyCodes[(int)state]))
        {
            switch (state)
            {
                case ETutorialState.STATE_RUN_RIGHT:
                    animator.Play("run_right", 0);
                    break;
                case ETutorialState.STATE_RUN_LEFT:
                    animator.Play("run_left", 0);
                    break;
                case ETutorialState.STATE_JUMP:
                    animator.Play("jump", 0);
                    break;
                case ETutorialState.STATE_FIREBALL:
                    animator.Play("fireball", 0);
                    animator_vuurbal.Play("afvuren",0);
                    break;
                case ETutorialState.STATE_PAUSE:
                    animator.Play("pauze", 0);
                    break;
                default:
                    break;
            }
        }

        //After five performances, go to next state
        if (gesturePerformances[(int)state] >= 5)
        {
            state++;
            gesturePlayer.clip = gestureClips[(int)state];
            tutorText.text = gestureStrings[(int)state];
        }

        //Check if the tutorial has ended
        if (state == ETutorialState.STATE_END)
        {
            //TODO: More fanfare
            SceneManager.LoadScene(0);
        }
    }
}
