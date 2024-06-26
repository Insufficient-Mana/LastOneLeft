using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//detection range, used for spawning window counterparts of overhead objects
[RequireComponent(typeof(Collider2D))]

/// <summary>
/// spawns and configures new window view objects when overhead objects are detected
/// </summary>
public class WndwObjGenerator_Overhead : MonoBehaviour
{
    [Header("CONFIG")]

    public GameObject windowEnvironmentParentObject;
    public GameObject windowAnchorObject;
    public float floorYValue = 0;

    [Header("DEBUG")]
    //used for ensuring each object only generate one window object
    public List<ObjectTracker_Overhead> overheadObjectsList;
    

    void Awake()
    {
        //set collider to be a trigger if not already set
        Collider2D myCollider = GetComponent<Collider2D>();
        if (!myCollider.isTrigger)
        {
            myCollider.isTrigger = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if object is in overhead and not already in the list of objects
        if (collision.gameObject.layer == 31) //31 is overhead layer
        {
            //attempt to fetch overhead object tracker script
            ObjectTracker_Overhead colObjectTracker = collision.GetComponent<ObjectTracker_Overhead>();

            //if overhead tracker is fetched successfully and it is NOT already in the list
            if (colObjectTracker != null &&
                !overheadObjectsList.Contains(colObjectTracker))
            {
                //add to list and create corresponding window object
                overheadObjectsList.Add(colObjectTracker);
                CreateWindowviewObject(colObjectTracker);

                //also register this anchor as the object's overhead anchor if not already registered
                if (colObjectTracker.overheadAnchorTransform != transform)
                {
                    colObjectTracker.overheadAnchorTransform = transform;
                }

            }

        }
    }

    /// <summary>
    /// when given an ObjectTracker_Overhead script, creates an object in the window view 
    /// that is synced to the tacview object's movements
    /// </summary>
    /// <param name="overheadTrackerScript"></param>
    public void CreateWindowviewObject(ObjectTracker_Overhead overheadTrackerScript)
    {
        GameObject parentObject;

        //make sure object spawns as child of correct parent in hierarchy
        if (overheadTrackerScript is ZombieTracker_Overhead)
        {
            //set parent object so zombie spawns as child of it's master object
            parentObject = (overheadTrackerScript as ZombieTracker_Overhead).ZmbMasterParentObj;
        }
        //if its not a zombie, make it a child of the window view obj
        else
        {
            parentObject = windowEnvironmentParentObject;
        }

        //instantiate new object in the window view and set its tracker script
        GameObject wndwObject = Instantiate(overheadTrackerScript.windowViewPrefab, parentObject.transform);
        wndwObject.GetComponent<PositionSync_Window>().overheadTrackerScript = overheadTrackerScript;

        //align bottom edge with ground y level
        wndwObject.transform.position = new Vector3(0, floorYValue, 0);

    }


}
