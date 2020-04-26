using Assets.Code;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionCallback : MonoBehaviour
{
    public GameObject calledObject;
    private CollisionCallable callableObject;

    // Start is called before the first frame update
    void Start()
    {
        callableObject = calledObject.GetComponent<CollisionCallable>();
        if (calledObject == null) Destroy(this);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter(Collision collision)
    {
        if (callableObject == null) callableObject = calledObject.GetComponent<CollisionCallable>();
        callableObject.OnCollisionEnter(collision);
    }

    void OnCollisionStay(Collision collision)
    {
        if (callableObject == null) callableObject = calledObject.GetComponent<CollisionCallable>();
        callableObject.OnCollisionStay(collision);
    }

    void OnCollisionExit(Collision collision)
    {
        if (callableObject == null) callableObject = calledObject.GetComponent<CollisionCallable>();
        callableObject.OnCollisionExit(collision);
    }

    void OnTriggerEnter(Collider collider)
    {
        if (callableObject == null) callableObject = calledObject.GetComponent<CollisionCallable>();
        callableObject.OnTriggerEnter(collider);
    }
}
