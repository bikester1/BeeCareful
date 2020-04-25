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
    private Sprite inventoryIcon;


    public float LowerBobLimit { get => lowerBobLimit; set => lowerBobLimit = value; }
    public float UpperBobLimit { get => upperBobLimit; set => upperBobLimit = value; }
    public float Rotation { get => rotation; set => rotation = value; }
    public MeshRenderer MeshRenderer { get => myMeshRenderer; set => myMeshRenderer = value; }
    public Sprite InventoryIcon { get => inventoryIcon; set => inventoryIcon = value; }

    public void UseItem()
    {
        throw new System.NotImplementedException();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
