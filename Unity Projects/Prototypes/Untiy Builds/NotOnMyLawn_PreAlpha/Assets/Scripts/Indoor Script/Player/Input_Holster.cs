using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// checks holster input and draws/holsters gun accordingly
/// </summary>
public class Input_Holster : MonoBehaviour
{
    public Player_States playerStates;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            //holster gun if drawn
            if (playerStates.gunIsDrawn)
            {
                playerStates.gunIsDrawn = false;
            }
            //draw gun if not drawn
            else if (!playerStates.gunIsDrawn)
            {
                playerStates.gunIsDrawn = true;
            }
        }
    }

}
