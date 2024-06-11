using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemSlot_UI : MonoBehaviour, IPointerEnterHandler,IPointerDownHandler,IPointerExitHandler,IPointerUpHandler
{
    public Backpack_UI Backpack;
    public bool isEmpty;
    public bool isFull;

    public int maxAmount;
    public int currentAmount;

    public int slotID;
    public bool isRadialSlot;

    public string itemName;
    [SerializeField] public int maxUses;
    [SerializeField] public Sprite iconSprite;

    // Start is called before the first frame update
    void Start()
    {
        isEmpty = true;
        isFull = false;
        currentAmount = 0;
    }

    #region Item Slot Actions
    /// <summary>
    /// Add the item to the slot
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public bool AddItem(GameObject item)
    {
        //Depending on the availability of the inventory slot it chooses a way of handeling adding an item
        if(isEmpty)
        {
            //recieves the incoming item's data and passes it along to slot
            Data_Item itemData = item.GetComponent<Data_Item>();
            itemName = itemData.itemName;
            maxUses = itemData.maxUses;
            iconSprite = itemData.iconSprite;
            maxAmount = itemData.maxStackable;
            SetIcon(iconSprite);
            currentAmount += 1;

            //Chanes the availability of the slot so that new items 
            isEmpty = false;
            checkFull();
            return true;
        }
        //When the slot is already populated by another item but it is not full
        else if(!isEmpty && !isFull)
        {
            Data_Item itemInfo = item.GetComponent<Data_Item>();
            //It will add one to the number of items in the slot
            if(itemName == itemInfo.itemName)
            {
                currentAmount += 1;
                checkFull();
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Function that uses the item in the slot 
    /// It will probably connect to an external script/scripts
    /// that hold all the information that allows the player to use the item
    /// </summary>
    public void UseItem()
    {
        Debug.Log("Use " + itemName);
    }

#endregion
    #region Slot Maintanance
    /// <summary>
    /// A function that checks whether the slot is full or not
    /// typically only used when adding items to the slot
    /// </summary>
    public void checkFull()
    {

        //max amount of supplies will be passed down from the item details in Data_Item
        if(currentAmount >= maxAmount)
        {
            isFull = true;
        }
        else if(currentAmount < maxAmount)
        {
            isFull = false;
        }
    }

    /// <summary>
    /// Clears the slot so that it can be used by another item when the
    /// item is used or dropped
    /// </summary>
    public void clearSlot()
    {
        itemName = null;
        maxUses = 0;
        iconSprite = null;
        maxAmount = 0;
        currentAmount = 0;
    }

    /// <summary>
    /// Used to set the profile icon for the slot
    /// </summary>
    /// <param name="spr"></param>
    public void SetIcon(Sprite spr)
    {
        Image image = GetComponent<Image>();
        if (image != null)
        {
            image.sprite = spr;
        }
    }
    #endregion
    #region Slot Interaction Events
    
    /// <summary>
    /// Tracks if the mouse is over a slot and it is not already holding a slot
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        //Only works with non radial menu slots
        if (!isRadialSlot)
        {
            //When the player is not dragging the same slot then it will
            //change the over slot item slot in the backpack to the one you are hovering over
            if (Backpack.draggedSlot != gameObject)
            {
                Backpack.overSlot = gameObject;
            }
        }
    }

    /// <summary>
    /// When the player clicks down on the menu slot and doesnt let go
    /// then it will "drag" the item slot around
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerDown(PointerEventData eventData)
    {
        //Only works with non radial menu slots
        if (!isRadialSlot)
        {
            //If the player is clicks on it while the object is already in the overslot
            //then it wil convert the oveer slot to a null to be used by a different slot
            if (Backpack.overSlot == gameObject)
            {
                Backpack.overSlot = null;
            }
            Backpack.draggedSlot = gameObject;
        }
    }

    /// <summary>
    /// When the player exits the area of the item slot
    /// it will update the overslot to null
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isRadialSlot)
        {
            if (Backpack.overSlot == gameObject)
            {
                Backpack.overSlot = null;
            }
        }
    }

    /// <summary>
    /// When the player lets go of the slot if the pointer is over
    /// a different slot then they will exchange data between the
    /// the two item slots
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerUp(PointerEventData eventData)
    {
        if (!isRadialSlot)
        {
            if (Backpack.overSlot != null && Backpack.draggedSlot != null)
            {
                //The overslot and the dragged info slots swap information
                ItemSlot_UI overSlotInfo = Backpack.overSlot.GetComponent<ItemSlot_UI>();
                ItemSlot_UI draggedSlotInfo = Backpack.draggedSlot.GetComponent<ItemSlot_UI>();
                Backpack.SwapSlots(overSlotInfo, draggedSlotInfo);
            }
            Backpack.draggedSlot = null;
        }
    }
    #endregion
}
