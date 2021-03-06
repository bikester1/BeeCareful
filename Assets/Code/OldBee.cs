﻿using Assets.Code;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class OldBee : Entity, Debuggable
{

    //General
    public Rigidbody beeRigidBody;
    public float forwardSpeed;
    public float attackSpeed;
    private int behaviorType;
    public float detectionRadius;

    //Bee memory
    [SerializeField]
    private Queue<Plant> memory;
    private Queue<float> memoryTimes;
    public float beeForgetTime;

    //Used for determining bee's next target
    private bool hasPollinated;
    public int maxPollinationCount;
    private int pollinationCount;
    private Vector3 target;
    private GameObject targetObject;
    private GameObject player;
    public float aggroRange;

    //Configures how much bee moves around.
    public float verticalSwaySpeed; //How fast bee sways up and down
    public float sidewaysSwaySpeed; //How fast bee sways side to side
    public float verticalAmplitude; //How far up and down bee moves
    public float horizontalAmplitude; //How far side to side bee moves
    private Vector3 tempSwayData; //Used to store sway data

    //Needed for Random Movement
    private float targetTimer;
    public float maxRandTime;
    public int findNewTarget;

    //Bee close to flower
    public float targetRange;
    public float heightDisplacement; //Height above plant bee should hover

    //Hover Stuff
    public float verticalHoverAmplitude;
    public float sidewaysHoverAmplitude;
    public float hoverVerticalSpeed;
    public float hoverSidewaysSpeed;
    private float randomOffset;
    Plant targetPlant;

    void Start()
    {
        //initiallizations when bee spawns
        hasPollinated = false;
        pollinationCount = 0;
        tempSwayData = Vector3.zero;
        randomOffset = UnityEngine.Random.Range(0f,2*Mathf.PI);
        player = GameObject.FindGameObjectWithTag("Player");

        memory = new Queue<Plant>();
        memoryTimes = new Queue<float>();

        //Finds the bee a target
        behaviorType = 1;

    }

    void Update()
    {
        
        //switch statement calling type of behavior
        switch (behaviorType)
        {
            case 1: //Looking for target - CHANGED TO ORIENTATION FLIGHTS
                LookForTarget();
                break;
            case 2: //CHANGE TO ORIENTATION FLIGHTS
                MoveToTarget();
                break;

            case 3: //Bee is within range of target
                CloseToTarget();
                break;

            case 4:
                HoverOverPlant();
                break;
            case 5://tracking Player
                trackPlayer();
                LookForTarget();
                break;
            case 6://random target
                MoveToRandom();
                break;
            case 7://Attacking Player
                attackPlayer();
                LookForTarget();
                break;
        }

        if (memory.Count > 0 && Time.realtimeSinceStartup - memoryTimes.Peek() > beeForgetTime)
        {
            memory.Dequeue();
            memoryTimes.Dequeue();
        }
        Debug.DrawLine(transform.position, target);
    }

    private void MoveToRandom()
    {
        targetTimer += Time.deltaTime;

        if(targetTimer > maxRandTime)
        {
            LookForTarget();
            targetTimer = 0;
        }

        beeRigidBody.transform.LookAt(target);
        beeRigidBody.velocity = transform.forward.normalized * forwardSpeed;
        tempSwayData.x = Mathf.Cos(Time.realtimeSinceStartup * sidewaysSwaySpeed * randomOffset) * horizontalAmplitude;
        tempSwayData.y = Mathf.Sin(Time.realtimeSinceStartup * verticalSwaySpeed * randomOffset) * verticalAmplitude;
        beeRigidBody.velocity += tempSwayData;
    }


    // Only call this function when you actually want a new target
    // for current target use Vec3 target
    void LookForTarget()
    {

        float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
        if (distanceToPlayer < aggroRange)
        {
            targetObject = player;
            target = player.transform.transform.position;
            behaviorType = 5;
            return;
        }

        GameObject closestHive = FindNearstOfType("Hive");
        if (closestHive != null && pollinationCount == maxPollinationCount)
        {
            behaviorType = 2;
            targetObject = closestHive;
            
            target = closestHive.transform.position;
            return;
        }

        GameObject flower = FindNearstOfType("Plant");
        if (flower != null && Vector3.Distance(flower.transform.position, transform.position) < detectionRadius)
        {
            behaviorType = 2;
            targetObject = flower;
            target = flower.transform.position + (Vector3.up * 2);
        } else
        {
            RandomTarget();
            behaviorType = 6;// random target
        }
    }

    void RandomTarget ()
    {
        float myX = transform.position.x;
        float myZ = transform.position.z;

        float xPos = UnityEngine.Random.Range(myX - 50, myX + 50);
        float zPos = UnityEngine.Random.Range(myZ - 50, myZ + 50);

        target = new Vector3(xPos, gameObject.transform.position.y, zPos);
    }

    void MoveToTarget()
    {
        beeRigidBody.transform.LookAt(target);
        beeRigidBody.velocity = transform.forward.normalized * forwardSpeed;
        tempSwayData.x = Mathf.Cos(Time.realtimeSinceStartup * sidewaysSwaySpeed + randomOffset) * horizontalAmplitude;
        tempSwayData.y = Mathf.Sin(Time.realtimeSinceStartup * verticalSwaySpeed + randomOffset) * verticalAmplitude;
        beeRigidBody.velocity += tempSwayData;

        if (Vector3.Distance(target, transform.position)<targetRange)
        {
            behaviorType = 3;
        }
    }

    void trackPlayer() 
    {
        beeRigidBody.transform.LookAt(target);
        beeRigidBody.velocity = transform.forward.normalized * forwardSpeed;
        tempSwayData.x = Mathf.Cos(Time.realtimeSinceStartup * sidewaysSwaySpeed * randomOffset) * horizontalAmplitude;
        tempSwayData.y = Mathf.Sin(Time.realtimeSinceStartup * verticalSwaySpeed * randomOffset) * verticalAmplitude;
        beeRigidBody.velocity += tempSwayData;

        if (Vector3.Distance(target, transform.position)<10)
        {
            target = GetComponent<Player>().getPlayerPosition();
            behaviorType = 7;
        }
    }

    void attackPlayer() //lock mode
    {

        beeRigidBody.transform.LookAt(target);
        beeRigidBody.velocity = transform.forward.normalized * attackSpeed;
    }

    void CloseToTarget()
    {
        beeRigidBody.transform.LookAt(target);
        beeRigidBody.velocity = transform.forward.normalized * forwardSpeed;
        tempSwayData.y = Mathf.Sin(Time.realtimeSinceStartup * verticalSwaySpeed + randomOffset) * verticalAmplitude;
        beeRigidBody.velocity += tempSwayData;
        float upperX = target.x + .1f;
        float lowerX = target.x - .1f;
        float upperZ = target.z + .1f;
        float lowerZ = target.z - .1f;
        if (transform.position.x <= upperX && transform.position.x >= lowerX && transform.position.z <= upperZ && transform.position.z >= lowerZ)
        {
            //CHANGE PLANT BOOL INRANGE TO TRUE
            behaviorType = 4;
        }
    }

    void HoverOverPlant()
    {
        tempSwayData.x = Mathf.Sin(Time.realtimeSinceStartup * hoverSidewaysSpeed + randomOffset) * sidewaysHoverAmplitude;
        tempSwayData.z = Mathf.Cos(Time.realtimeSinceStartup * hoverSidewaysSpeed + randomOffset) * sidewaysHoverAmplitude;
        tempSwayData.y = Mathf.Sin(Time.realtimeSinceStartup * verticalSwaySpeed + randomOffset) * verticalHoverAmplitude;
        beeRigidBody.velocity = tempSwayData;
        beeRigidBody.transform.LookAt(target);


    }

    public void changeBehaviorType(int x)
    {
        behaviorType = x;
    }

    public void OnCollisionEnter(Collision collision)
    {
        ContactPoint[] contacts = new ContactPoint[collision.contactCount];
        collision.GetContacts(contacts);

        for(int i = 0; i < collision.contactCount; i++)
        {
            if (contacts[i].otherCollider.transform.tag.Contains("Player"))
            {
                Destroy(this.gameObject);
                Destroy(this);
            }
            if (contacts[i].otherCollider.transform.tag.Contains("Hive"))
            {
                Destroy(this.gameObject);
                Destroy(this);
                targetObject.GetComponent<Hive>().beeCount++;
            }
        }
    }

    public GameObject FindNearstOfType(string type)
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag(type);
        GameObject closestObject = null;
        float leastDistance = Mathf.Infinity;
        float tempDistance;
        foreach (GameObject obj in objects)
        {
            tempDistance = Vector3.Distance(obj.transform.position, transform.position);
            if (tempDistance < leastDistance && !memory.Contains(obj.GetComponent<Plant>()))
            {
                leastDistance = tempDistance;
                closestObject = obj;
            }
        }

        return closestObject;
    }

    public void pollinated()
    {
        pollinationCount++;
    }

    public void AddPlantToMemory(Plant p)
    {
        memory.Enqueue(p);
        memoryTimes.Enqueue(Time.realtimeSinceStartup);
    }

    public string GetDebugInfo()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("Bee Object:" + this.GetHashCode());
        sb.Append("\nNext Memory Forgotten:" + (memoryTimes.Count > 0 ? beeForgetTime - (Time.realtimeSinceStartup - memoryTimes.Peek()) : -1));
        sb.Append("\nTarget Timer" + (maxRandTime - targetTimer));
        sb.Append("\nBehaviour State: " + behaviorType);
        return sb.ToString();
    }
}
