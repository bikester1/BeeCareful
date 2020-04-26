using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Item item;

    public Inventory inventory;
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

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("Pointer Up Event: " + eventData.selectedObject.GetHashCode());
    }
}
