using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows.WebCam;

public class Seed : Item
{

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


    private PrefabManager prefabManager;

    public override void UseItem(GameObject gameObject)
    {
        throw new System.NotImplementedException();
    }

    // Start is called before the first frame update
    void Start()
    {
        myMeshRenderers = GetComponentsInChildren<MeshRenderer>();
        inventoryIcon = GetComponentInChildren<Image>().gameObject;
        prefabManager = GameObject.FindObjectOfType<PrefabManager>();
        //inventoryIcon = prefabManager.seedIcon;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
