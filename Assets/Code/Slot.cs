using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Item item;

    public Inventory inventory;

    public EventSystem eventSystem;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Pointer Down Event");
    }

    // transfer item that was being dragged
    public void OnPointerUp(PointerEventData eventData)
    {
        foreach(GameObject gameObject in eventData.hovered)
        {
            // if a slot is being hovered over
            if(gameObject.GetComponent<Slot>() != null)
            {
                Slot hoveredSlot = gameObject.GetComponent<Slot>();
                Item tempItem = hoveredSlot.item;
                this.item = hoveredSlot.item;
                hoveredSlot.item = tempItem;

                // Attach icon to new slot
                hoveredSlot.item.InstantiatedInventoryIcon.transform.parent = hoveredSlot.transform;
                hoveredSlot.item.InstantiatedInventoryIcon.transform.localPosition = Vector3.zero;

                // attach replaced item to new slot
                if(this.item != null)
                {
                    this.item.InstantiatedInventoryIcon.transform.parent = this.transform;
                    this.item.InstantiatedInventoryIcon.transform.localPosition = Vector3.zero;
                }
            }
        }
    }
}
