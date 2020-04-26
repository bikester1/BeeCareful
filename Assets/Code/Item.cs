using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;



public abstract class Item : MonoBehaviour
{
    public abstract float LowerBobLimit { get; set; }
    public abstract float UpperBobLimit { get; set; }
    public abstract float Rotation { get; set; }
    public abstract MeshRenderer MeshRenderer { get; set; }
    public abstract GameObject InventoryIcon { get; set; }
    public abstract GameObject InstantiatedInventoryIcon { get; set; }

    public abstract void UseItem();
}

