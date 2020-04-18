using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    CapsuleCollider collisionCap;
    Camera cam;
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        collisionCap = GetComponentInChildren<CapsuleCollider>();
        cam = this.GetComponentInChildren<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessInput();
    }


    void ProcessInput()
    {
        #region Horizontal Movement
        if (Input.GetKey(KeyCode.A))
        {
            this.transform.position += cam.transform.rotation * Vector3.left * speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D))
        {
            this.transform.position += cam.transform.rotation * Vector3.right * speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.W))
        {
            this.transform.position += cam.transform.rotation * Vector3.forward * speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S))
        {
            this.transform.position += cam.transform.rotation * Vector3.back * speed * Time.deltaTime;
        }
        #endregion

        #region Vetical Movement
        if (Input.GetKey(KeyCode.LeftControl))
        {
            this.transform.position += cam.transform.rotation * Vector3.down * speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.Space))
        {
            this.transform.position += cam.transform.rotation * Vector3.up * speed * Time.deltaTime;
        }
        #endregion

        if (Input.GetKeyDown(KeyCode.L))
        {
            Debug.Log(cam.transform.rotation.eulerAngles);
        }
    }
}
