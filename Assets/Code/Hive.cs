using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hive : MonoBehaviour
{
    public PrefabManager prefab;
    private GameObject newBee;
    private Vector3 spawnPosition;
    public float spawnOffset;
    public int beeCount;
    public Bee returningBee;
    public float beeCooldownLength;
    private float beeCooldownTimer;

    // Start is called before the first frame update
    void Start()
    {
        spawnPosition = transform.rotation * Vector3.forward * spawnOffset;
        spawnPosition += transform.position;
        beeCount = 2;
    }

    // Update is called once per frame
    void Update()
    {
        if (beeCount>0)
        {
            beeCooldown();
        }
        
    }

    void SpawnBee()
    {
        newBee = Instantiate(prefab.bee);
        newBee.GetComponent<Transform>().position = spawnPosition;
        newBee.GetComponent<Bee>().HomeHive = this;
        beeCount--;
    }

    void beeCooldown()
    {
        beeCooldownTimer += Time.deltaTime;
        if (beeCooldownTimer == beeCooldownLength)
        {
            SpawnBee();
            beeCooldownTimer = 0;
        }
    }
}
