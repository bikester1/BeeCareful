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
                Item tempItem = this.item;
                this.item = hoveredSlot.item;
                hoveredSlot.item = tempItem;

                // Attach icon to new slot
                if (hoveredSlot.item != null)
                {
                    hoveredSlot.item.SetParentSlot(hoveredSlot);
                }

                // attach replaced item to new slot
                if(this.item != null)
                {
                    this.item.SetParentSlot(this);
                }
            }
        }
    }
}
