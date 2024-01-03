using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private int[] gesturePerformances = {0, 0, 0, 0, 0, 0};

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(gestureKeyCodes[(int)state]))
            gesturePerformances[(int)state]++;

        //After five performances, go to next state
        if (gesturePerformances[(int)state] >= 5)
            state++;

        if (state == ETutorialState.STATE_END)
        {
            Debug.Log("HURRAY!!!");
            
        }

    }
}
