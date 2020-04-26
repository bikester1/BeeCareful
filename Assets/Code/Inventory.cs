using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public GameObject slotParent;
    public Slot[] slots;

    public int numSlots;

    private Canvas myCanvas;
    


    public void Start()
    {
        slots = new Slot[numSlots];
        myCanvas = GetComponent<Canvas>();

        slots = slotParent.GetComponentsInChildren<Slot>();
    }

    public void Update()
    {
        for (int i = 0; i < numSlots; i++)
        {
            if(slots[i].item == null)continue;
            if (slots[i].item.InstantiatedInventoryIcon == null)
            {
                slots[i].item.InstantiatedInventoryIcon = Instantiate(slots[i].item.InventoryIcon);
                slots[i].item.InstantiatedInventoryIcon.transform.SetParent(slots.ElementAt(i).transform);
                slots[i].item.InstantiatedInventoryIcon.transform.localPosition = Vector3.zero;
            }

        }

        //buttons[10].
    }

    public void ClickedSlot(int slotNum)
    {
        // do nothing if no item.
        if (slots[slotNum].item == null) return;


    }

    public int FirstEmptySlot()
    {

        for (int i = 0; i < numSlots; i++)
        {
            if (slots[i].item == null) return i;
        }

        return -1;
    }

    public void ItemToFirstAvailableSlot(Item item)
    {
        int slot = FirstEmptySlot();


    }

    // place item in slot
    // if an Item is already in that slot it will return the item 
    public Item ItemToSlot(Item item, int slot)
    {
        Item retItem = slots[slot].item;
        slots[slot].item = item;
        return retItem;
    }

}

