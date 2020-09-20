using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows.WebCam;

public class Bucket : Item
{

    private float hydration = 1000;
    private float pourSize = 180;
    private float maxFill = 2000;

    private float lowerBobLimit;
    private float upperBobLimit;
    private float rotation;
    private bool inInventory;
    private MeshRenderer[] myMeshRenderers;
    private GameObject inventoryIcon;
    private GameObject instantiatedInventoryIcon;


    public override float LowerBobLimit { get => lowerBobLimit; set => lowerBobLimit = value; }
    public override float UpperBobLimit { get => upperBobLimit; set => upperBobLimit = value; }
    public override float Rotation { get => rotation; set => rotation = value; }
    public override MeshRenderer[] MeshRenderers => myMeshRenderers;
    public override GameObject InventoryIcon => inventoryIcon;
    public override GameObject InstantiatedInventoryIcon { get => instantiatedInventoryIcon; protected set => instantiatedInventoryIcon = value;  }
    public override bool isInInventory => inInventory;

    /// <summary>
    /// depending on the object passed this item will fill the bucket or water a plant
    /// </summary>
    /// <param name="gameObject"></param>
    public override void UseItem(GameObject gameObject)
    {
        if (gameObject.GetComponentInParent<Plant>() != null)
        {
            if (hydration >= pourSize) hydration -= gameObject.GetComponentInParent<Plant>().WaterPlant(pourSize);
            return;
        }

        if(gameObject.GetComponentInChildren<Well>() != null)
        {
            if (hydration == maxFill) return;

            if(hydration + pourSize <= maxFill)
            {
                hydration += pourSize;
            }
            else
            {
                hydration = maxFill;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        myMeshRenderers = GetComponentsInChildren<MeshRenderer>();
        inventoryIcon = GetComponentInChildren<Image>().gameObject;
        //inventoryIcon = prefabManager.seedIcon;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
