using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interaction_Player : MonoBehaviour
{
    [Header("DEBUG")]
    public List<GameObject> itemsInField;
    public bool itemInRange;
    public Vector3 playerPosition;
    public GameObject Inventory;
    public GameObject closetItem;
    public float lastItem;
    public int itemLength;
    public int closestElement;
    public bool interacting;
    public int interactionType;

    public bool maxxedInventory;

    //establishes that there is no item to begin with
    void Start()
    {
        itemInRange = false;

    }
    // Update is called once per frame
    void Update()
    {
        //keeps track of the player position and an interactable is in range it finds the closest interactable and identifies if its an item or enviornment
        playerPosition = transform.position;
        if (itemInRange)
        {
            FindClosestItem();
            InteractableIdentification();
        }
        else
        {
            interacting = false;
        }
    }

    /// <summary>
    /// once an interactable is in range then the item is added to the list of close objects
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //once the interactable is in range it adds it to the list
        if (collision.gameObject.layer == LayerMask.NameToLayer("Interactable"))
        {
            itemsInField.Add(collision.gameObject);
        }
    }

    /// <summary>
    /// As long as there is an item in range then it will be interactable
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerStay2D(Collider2D collision)
    {
        //if a collision is an interactable then it sets in range to true
        if (collision.gameObject.layer == LayerMask.NameToLayer("Interactable"))
        {
            itemInRange = true;
        }

    }

    /// <summary>
    /// Once the item leaves the field then it is removed from the list
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerExit2D(Collider2D collision)
    {
        //if an item leaves it sets the in range to false and removes objects from the list
        if (collision.gameObject.layer == LayerMask.NameToLayer("Interactable"))
        {
            itemInRange = false;
            itemsInField.Remove(collision.gameObject);
        }
    }

    /// <summary>
    /// A function to find the closest interactable
    /// </summary>
    public void FindClosestItem()
    {
        //gets the list length and sets what the last item is to infinity inorder to ensure no item is bigger than the initial closest item
        itemLength = itemsInField.Count;
        lastItem = Mathf.Infinity;

        //goes through each interactable and determines the closest interactable
        for (int i = 0; i < itemLength; i++)
        {
            //triangulates the actual closest distance using pythag theorum
            Vector3 distanceFromPlayer = itemsInField[i].transform.position - playerPosition;
            float distance;
            distance = CalculateDistance(distanceFromPlayer);

            //if the current distance is less then the last item then it is the closest
            if (distance < lastItem)
            {
                closetItem = itemsInField[i];
                closestElement = i;
                lastItem = distance;
            }
        }
        lastItem = Mathf.Infinity;
    }

    /// <summary>
    /// calculates the distance of one object from another
    /// </summary>
    /// <param name="distance"></param>
    /// <returns></returns>
    public float CalculateDistance(Vector3 distance)
    {
        float pythagDistance;
        pythagDistance = Mathf.Sqrt((distance.x * distance.x) + (distance.y * distance.y));

        return pythagDistance;
    }

    /// <summary>
    /// once the closest item is found this function determines whether the interactable is item or enviornment
    /// </summary>
    public void InteractableIdentification()
    {
        InteractionIdentification_Item Interactable = closetItem.GetComponent<InteractionIdentification_Item>();
        Inventory_Player inventoryStorage = Inventory.GetComponent<Inventory_Player>();
        //if the interaction is an item it puts it in the inventory
        if (Interactable.isItem && !Interactable.isEnviormentObject && interacting)
        {
            ItemInteraction_Player itemAssignment = GetComponent<ItemInteraction_Player>();
            Interaction_Player ItemInteraction = GetComponent<Interaction_Player>();

            //Changes the interaction pathway if the item is a weapon
            if (Interactable.isGun || Interactable.isMeleeWeapon)
            {
                inventoryStorage.itemAssignment = itemAssignment;
                inventoryStorage.ItemInteractionScript = ItemInteraction;
                inventoryStorage.StoreItems(closetItem);
                interacting = false;
            }
            else
            {
                //Sends any materials or items to the backpack
                Backpack_UI backpackInfo = inventoryStorage.Backpack.GetComponent<Backpack_UI>();
                backpackInfo.AddItem(closetItem);
                interacting = false;
            }
            
        }
        //if the interaction is an envionrmental object it interacts with it
        else if (!Interactable.isItem && Interactable.isEnviormentObject && interacting)
        {
            EnviornmentInteraction_Player enviornment = GetComponent<EnviornmentInteraction_Player>();
            enviornment.Interact(closetItem, interactionType);
            //Uses the interaction types to determine when to stop the interaction
            if(interactionType == 0 || interactionType == 2)
            {
                interacting = false;
            }
            else if(interactionType == 1)
            {
                interacting = true;
            }
        }

    }

    /// <summary>
    /// Allows the player to interact with items and enviornment by 
    /// pressing E on the keyboard
    /// </summary>
    /// <param name="actionContext"></param>
    public void InteractionAction(InputAction.CallbackContext actionContext)
    {
        if (actionContext.started)
        {
            interactionType = 0;
            interacting = true;
        }

    }

    /// <summary>
    /// Allows the player to hold down on W to do a different type of interaction 
    /// in case enviornmental objects have more than 1 type of interaction
    /// </summary>
    /// <param name="actionContext"></param>
    public void SecondaryInteractionAction(InputAction.CallbackContext actionContext)
    {

        if (actionContext.performed)
        {
            interactionType = 1;
            interacting = true;
            Debug.Log("Interaction");
        }
        else if (actionContext.canceled)
        {
            interactionType = 2;
            interacting = true;
        }
    }
}
