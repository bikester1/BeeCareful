using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Transform transform;
    CapsuleCollider collisionCap;

    // Start is called before the first frame update
    void Start()
    {
        transform = GetComponent<Transform>();
        collisionCap = transform.GetChild(1).GetComponent<CapsuleCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Left"))
        {
            Debug.Log("Left");
        }
    }
}
