using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public abstract class Item : MonoBehaviour
{
    public abstract float LowerBobLimit { get; set; }
    public abstract float UpperBobLimit { get; set; }
    public abstract float Rotation { get; set; }
    public abstract MeshRenderer MeshRenderer { get; }
    public abstract GameObject InventoryIcon { get; }
    public abstract GameObject InstantiatedInventoryIcon { get; protected set; }
    public abstract bool isInInventory { get; }

    public abstract void UseItem();

    public virtual void SendToInventory(Inventory inv) 
    {
        int slot = inv.FirstEmptySlot();

        // inventory full
        if (slot == -1)
        {
            return;
        }

        this.MeshRenderer.enabled = false;
        this.GetComponent<Collider>().enabled = false;
        inv.ItemToSlot(this, slot);

        SetParentSlot(inv.Slots[slot]);
        InventoryIcon.GetComponent<Image>().enabled = true;
    }

    public virtual void SetParentSlot(Slot slot)
    {
        InventoryIcon.transform.SetParent(slot.transform);
        InventoryIcon.transform.localPosition = Vector3.zero;
        InventoryIcon.transform.localScale = Vector3.one;
        InventoryIcon.transform.localRotation = Quaternion.Euler(Vector3.zero);
    }

}

