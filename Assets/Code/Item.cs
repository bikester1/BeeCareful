using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;



public interface Item
{
    float LowerBobLimit { get; set; }
    float UpperBobLimit { get; set; }
    float Rotation { get; set; }
    MeshRenderer MeshRenderer { get; set; }
    GameObject InventoryIcon { get; set; }
    GameObject InstantiatedInventoryIcon { get; set; }

    void UseItem();
}

