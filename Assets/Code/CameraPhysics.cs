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
    public Rigidbody targetRigidbody;
    public float lookSensitivity = 1;
 
    // Start is called before the first frame update
    void Start()
    {
        // set distance
        this.transform.position = this.position.normalized * position.z;
        targetRigidbody = transform.parent.GetComponentInChildren<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            position.x += (Input.GetAxisRaw("Mouse X") * lookSensitivity);
            Debug.Log("Broken: " + Input.GetAxisRaw("Mouse X") * lookSensitivity);
            Debug.Log("Working: " + Input.GetAxisRaw("Mouse X") * 3);
            position.y -= Input.GetAxisRaw("Mouse Y");
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            position.x -= 1f;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            position.x += 1f;
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            position.y += 1f;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            position.y -= 1f;
        }


        this.transform.position = targetRigidbody.position + Quaternion.Euler(position.y, position.x, 0) * new Vector3(0, 0, -position.z);
        this.transform.LookAt(targetRigidbody.position, Vector3.up);
    }
}
