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
    private Camera myCam;
    private bool planting = false;
 
    // Start is called before the first frame update
    void Start()
    {
        // set distance
        this.transform.position = this.position.normalized * position.z;
        targetRigidbody = transform.parent.GetComponentInChildren<Rigidbody>();
        myCam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            position.x += (Input.GetAxisRaw("Mouse X") * lookSensitivity);
            position.y -= (Input.GetAxisRaw("Mouse Y") * lookSensitivity);
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

        if (planting) UpdatePlant();
    }

    void UpdatePlant()
    {
        Ray ray = myCam.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
        Debug.DrawRay(ray.origin, ray.direction * 10, Color.white);

        RaycastHit hit;
        

        if(Physics.Raycast(ray, out hit, 15f))
        {

        }

        if (Input.GetMouseButton(0))
        {

        }
    }

    public void placePlant()
    {
        planting = true;
        Debug.Log("planting Started");
    }
}
