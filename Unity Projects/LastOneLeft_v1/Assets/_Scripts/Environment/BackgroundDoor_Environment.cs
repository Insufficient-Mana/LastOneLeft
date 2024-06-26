using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundDoor_Environment : MonoBehaviour
{
    //assigns the doors in the set
    public GameObject door1;
    public GameObject door2;

    //defines which door is currently being used
    public GameObject currentDoor;

    //Defines which room that the door is in
    public GameObject inRoom;

    //Holds the value that player will teleport to from side doors
    public float doorOffset;
}
