using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.WebCam;

public class Seed : MonoBehaviour, Item
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


    public float LowerBobLimit { get => lowerBobLimit; set => lowerBobLimit = value; }
    public float UpperBobLimit { get => upperBobLimit; set => upperBobLimit = value; }
    public float Rotation { get => rotation; set => rotation = value; }
    public MeshRenderer MeshRenderer { get => myMeshRenderer; set => myMeshRenderer = value; }
    public GameObject InventoryIcon { get => inventoryIcon; set => inventoryIcon = value; }
    public GameObject InstantiatedInventoryIcon { get => instantiatedInventoryIcon; set => instantiatedInventoryIcon = value; }

    private PrefabManager prefabManager;

    public void UseItem()
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
