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
        // set distance
        this.transform.position = this.position.normalized * position.z;

    }

    // Update is called once per frame
    void Update()
    {
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


        this.transform.position = this.transform.parent.transform.position + Quaternion.Euler(position.y, position.x, 0) * new Vector3(0, 0, -position.z);
        this.transform.LookAt(this.transform.parent.transform.position, Vector3.up);
    }
}
