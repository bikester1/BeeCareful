using Assets.Code;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionCallback : MonoBehaviour
{
    public MonoBehaviour calledObject;
    private CollisionCallable callableObject;

    // Start is called before the first frame update
    void Start()
    {
        callableObject = calledObject as CollisionCallable;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision collision)
    {
        callableObject.OnCollisionEnter(collision);
    }

    void OnCollisionStay(Collision collision)
    {
        callableObject.OnCollisionStay(collision);
    }

    void OnCollisionExit(Collision collision)
    {
        callableObject.OnCollisionExit(collision);
    }
}
