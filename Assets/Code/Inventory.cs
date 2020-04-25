using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

class Inventory : MonoBehaviour
{
    private Item[] inventorySlots;
    public int numSlots;



    public void Start()
    {
        inventorySlots = new Item[numSlots];
    }

    public void Update()
    {
        
    }

    public void ClickedSlot(int slotNum)
    {
        // do nothing if no item.
        if (inventorySlots[slotNum] == null) return;


    }

    public int FirstEmptySlot()
    {

        for(int i = 0; i < numSlots; i++)
        {
            if (inventorySlots[i] != null) return i;
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
        Item retItem = inventorySlots[slot];
        inventorySlots[slot] = item;
        return retItem;
    }

}

