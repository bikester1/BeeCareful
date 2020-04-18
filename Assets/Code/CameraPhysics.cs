using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPhysics : MonoBehaviour
{
    // Specifies rotation around and above character as well as distance
    // x: rotation
    // y: height
    // z: distance
    public Vector3 position;
 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            Debug.Log(this.GetComponentInParent<Transform>().position);
            Debug.Log("Angle: " + this.transform.rotation.eulerAngles);
            this.transform.LookAt(this.transform.parent.transform.position, Vector3.up);
        }
        if (Input.GetKey(KeyCode.Mouse1))
        {
            Debug.Log(this.GetComponentInParent<Transform>().position);
            Debug.Log("Angle: " + this.transform.rotation.eulerAngles);
            this.transform.LookAt(this.transform.parent.transform.position, Vector3.up);
        }
    }
}
