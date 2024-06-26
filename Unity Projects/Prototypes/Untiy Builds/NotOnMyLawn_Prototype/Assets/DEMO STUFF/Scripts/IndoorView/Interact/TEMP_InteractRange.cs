using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TEMP_InteractRange : MonoBehaviour
{
    public int interactablesLayer = 8;
    public BoxCollider2D interactionHitbox;

    public GameObject[] interactablesInRange;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == interactablesLayer)
        {
            TEMP_InteractableStates stateHolder = collision.GetComponent<TEMP_InteractableStates>();
            stateHolder.isInRange = true;

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == interactablesLayer)
        {
            TEMP_InteractableStates stateHolder = collision.GetComponent<TEMP_InteractableStates>();
            stateHolder.isInRange = false;
            
        }

    }

}
