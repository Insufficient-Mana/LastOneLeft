using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player_InstantLRMovement : MonoBehaviour
{
    [Header("CONFIG")]
    public float drawnMoveSpeed = 2;
    public float holsteredMoveSpeed = 5;

    [Header("DEBUG")]
    public float inputDirection;
    public float currentMoveSpeed;

    public Rigidbody2D myRigidbody2D;
    public Player_States playerStates;
    public Player_PositionTracker playerPosition;

    void Start()
    {
        //fetch components
        myRigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        GetMovementInput();
        ProcessGunStatus();

        float newVelocity = inputDirection * currentMoveSpeed;
        UpdatePlayerStates(newVelocity);

        //apply movement to rigidbody
        myRigidbody2D.velocity = new Vector2(newVelocity, 0);

        //sends player's position to scriptable object
        playerPosition.playerPosition = transform.position;
    }

    /// <summary>
    /// clears and then sets input direction variable based on L/R input keys
    /// </summary>
    private void GetMovementInput()
    {
        inputDirection = 0;

        //L movement
        if (Input.GetKey(KeyCode.A))
        {
            inputDirection--;
        }
        //R movement
        if (Input.GetKey(KeyCode.D))
        {
            inputDirection++;
        }
    }

    /// <summary>
    /// sets speed based on gun's drawn/holstered status
    /// </summary>
    public void ProcessGunStatus()
    {
        if (playerStates.gunIsDrawn)
        {
            currentMoveSpeed = drawnMoveSpeed;
        }
        //if holstered
        else
        {
            currentMoveSpeed = holsteredMoveSpeed;
        }
    }
    
    /// <summary>
    /// Updates isWalking and movementDirection states
    /// </summary>
    /// <param name="newVelocity"></param>
    private void UpdatePlayerStates(float newVelocity)
    {
        if (newVelocity == 0)
        {
            playerStates.isWalking = false;
        }
        else
        {
            playerStates.isWalking = true;

            //update face direction when moving
            if (newVelocity > 0)
            {
                playerStates.faceDirection = 1;
            }
            else if (newVelocity < 0)
            {
                playerStates.faceDirection = -1;
            }
        }
    }
}
