using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

class Inventory : MonoBehaviour
{
    private Item[] inventorySlots;
    public Image[] Slots;
    public int numSlots;

    private Canvas myCanvas;
    


    public void Start()
    {
        inventorySlots = new Item[numSlots];
        myCanvas = GetComponent<Canvas>();
    }

    public void Update()
    {
        for (int i = 0; i < numSlots; i++)
        {
            if(inventorySlots[i] == null)continue;
            if (inventorySlots[i].InstantiatedInventoryIcon == null)
            {
                inventorySlots[i].InstantiatedInventoryIcon = Instantiate(inventorySlots[i].InventoryIcon);
                inventorySlots[i].InstantiatedInventoryIcon.transform.parent = Slots.ElementAt(i).transform;
                inventorySlots[i].InstantiatedInventoryIcon.transform.localPosition = Vector3.zero;
            }

        }
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
            if (inventorySlots[i] == null) return i;
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

