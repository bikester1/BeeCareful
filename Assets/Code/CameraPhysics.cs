﻿using Assets.Code;
using System.Collections;
using System.Collections.Generic;
using System.Security.AccessControl;
using UnityEngine;
using UnityEngine.Diagnostics;
using UnityEngine.UI;

public class CameraPhysics : MonoBehaviour
{
    // Specifies rotation around and above character as well as distance
    // x: rotation
    // y: height
    // z: distance
    public Vector3 position;
    public Rigidbody targetRigidbody;

    // public variables
    public float lookSensitivity = 1;
    public float zoomSensitivity = 3;
    public float plantingReach = 20f;

    private Camera myCam;
    private bool placing = false;
    private GameObject placeable;
    private Vector3 placementOffset;

    // needed to create prefabs
    private PrefabManager prefabManager;

    // Debug variables
    private Canvas myCanvas;
    private Text myDebugText;
    private Debuggable debugTarget;
 
    // Start is called before the first frame update
    void Start()
    {
        // set distance
        this.transform.position = this.position.normalized * position.z;
        targetRigidbody = transform.parent.GetComponentInChildren<Rigidbody>();
        myCam = GetComponent<Camera>();
        prefabManager = GameObject.FindObjectOfType<PrefabManager>();

        // Debug stuff
        myCanvas = myCam.GetComponentInChildren<Canvas>();
        myDebugText = myCanvas.GetComponentInChildren<Text>();

        // Free Sync Gsync stuff
        Application.targetFrameRate = -1;
        Debug.Log(QualitySettings.vSyncCount);
        QualitySettings.vSyncCount = 0;
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

        position.z -= Input.GetAxisRaw("Mouse ScrollWheel") * zoomSensitivity;

        // add camera position info to the targets position and then look at the target
        this.transform.position = targetRigidbody.position + Quaternion.Euler(position.y, position.x, 0) * new Vector3(0, 0, -position.z);
        this.transform.LookAt(targetRigidbody.position, Vector3.up);

        if (placeable == null) placing = false;
        if (placing) UpdatePlant();
        else FindDebugObject();

        if(debugTarget != null)
        {
            myDebugText.text = debugTarget.GetDebugInfo();
        }
    }

    private void UpdatePlant()
    {
        Ray ray = myCam.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
        Debug.DrawRay(ray.origin, ray.direction * 10, Color.white);

        RaycastHit hit;
        LayerMask mask = LayerMask.GetMask("Ground");


        if(Physics.Raycast(ray, out hit, Mathf.Infinity, mask))
        {

            if(hit.transform.tag.Contains("Plantable")) placeable.GetComponent<Transform>().position = hit.point + placementOffset;

        }

        if (Input.GetMouseButton(0))
        {
            placeable.GetComponent<MonoBehaviour>().enabled = true;
            if (placeable.GetComponent<Placeable>() != null) placeable.GetComponent<Placeable>().Place();
            placeable = null;
            placing = false;

        }
    }

    private void placePlant()
    {
        
        placing = true;
        placementOffset = Vector3.zero;
        placeable = Instantiate(prefabManager.flowerBasic);
        placeable.GetComponent<Transform>().position = new Vector3(0, -10000, 0);
        placeable.GetComponent<MonoBehaviour>().enabled = false;
    }

    private void placeBee()
    {
        placing = true;
        placementOffset = Vector3.up * 2;
        placeable = Instantiate(prefabManager.bee);
        placeable.GetComponent<Transform>().position = new Vector3(0, -10000, 0);
        placeable.GetComponent<MonoBehaviour>().enabled = false;
    }

    private void FindDebugObject()
    {
        Ray ray = myCam.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
        Debug.DrawRay(ray.origin, ray.direction * 10, Color.white);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            Debuggable debugObj = hit.transform.GetComponentInParent<Debuggable>();
            if(debugObj != null && Input.GetMouseButton(0))
            {
                Debug.Log("New Debug Target");
                debugTarget = debugObj;
            }

        }
    }

    public void RaycastForItemUse(Item item)
    {
        Ray ray = myCam.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            item.UseItem(hit.transform.gameObject);
        }
    }
}
