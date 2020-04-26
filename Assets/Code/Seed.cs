using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.WebCam;

public class Seed : Item
{

    [SerializeField]
    private float lowerBobLimit;
    [SerializeField]
    private float upperBobLimit;
    [SerializeField]
    private float rotation;
    [SerializeField]
    private MeshRenderer myMeshRenderer;
    [SerializeField]
    private GameObject inventoryIcon;
    [SerializeField]
    private GameObject instantiatedInventoryIcon;


    public override float LowerBobLimit { get => lowerBobLimit; set => lowerBobLimit = value; }
    public override float UpperBobLimit { get => upperBobLimit; set => upperBobLimit = value; }
    public override float Rotation { get => rotation; set => rotation = value; }
    public override MeshRenderer MeshRenderer { get => myMeshRenderer; set => myMeshRenderer = value; }
    public override GameObject InventoryIcon { get => inventoryIcon; set => inventoryIcon = value; }
    public override GameObject InstantiatedInventoryIcon { get => instantiatedInventoryIcon; set => instantiatedInventoryIcon = value; }

    private PrefabManager prefabManager;

    public override void UseItem()
    {
        throw new System.NotImplementedException();
    }

    // Start is called before the first frame update
    void Start()
    {
        prefabManager = GameObject.FindObjectOfType<PrefabManager>();
        inventoryIcon = prefabManager.seedIcon;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
