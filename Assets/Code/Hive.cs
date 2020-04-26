using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hive : MonoBehaviour
{
    public PrefabManager prefab;
    private GameObject newBee;
    private Vector3 spawnPosition;
    public float spawnOffset;

    // Start is called before the first frame update
    void Start()
    {
        spawnPosition = transform.rotation * Vector3.forward * spawnOffset;
        spawnPosition += transform.position;
        SpawnBee();


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnBee()
    {
        newBee = Instantiate(prefab.bee);
        newBee.GetComponent<Transform>().position = spawnPosition;
    }
}
