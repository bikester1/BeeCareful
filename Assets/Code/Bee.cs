using Assets.Code;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bee : Entity
{

    //General
    public Rigidbody beeRigidBody;
    public float forwardSpeed;
    private int behaviorType;
    public float detectionRadius;

    //Bee memory
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
            case 1: //Looking for target
                LookForTarget();
                break;
            case 2: //Bee finds plant
                MoveToTarget();
                break;

            case 3: //Bee is within range of target
                CloseToTarget();
                break;

            case 4:
                Hover();
                break;
            case 5://Attacking Player
                AttackPlayer();
                LookForTarget();
                break;
            case 6://random target
                MoveToRandom();
                break;
        }

        if (memory.Count > 0 && Time.realtimeSinceStartup - memoryTimes.Peek() > beeForgetTime)
        {
            Debug.Log(Time.realtimeSinceStartup);
            memory.Dequeue();
            memoryTimes.Dequeue();
        }
        Debug.DrawLine(transform.position, targetObject.transform.position);
        Debug.DrawLine(target, targetObject.transform.position);
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

        GameObject flower = FindNearstOfType("Plant");
        
        float distanceToFlower = Vector3.Distance(flower.transform.position, transform.position);
        if (distanceToFlower < detectionRadius)
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
            Debug.Log("SWITCHED TO HOVER");
            behaviorType = 3;
        }
    }

    void AttackPlayer()
    {
        beeRigidBody.transform.LookAt(target);
        beeRigidBody.velocity = transform.forward.normalized * forwardSpeed;
        tempSwayData.x = Mathf.Cos(Time.realtimeSinceStartup * sidewaysSwaySpeed * randomOffset) * horizontalAmplitude;
        tempSwayData.y = Mathf.Sin(Time.realtimeSinceStartup * verticalSwaySpeed * randomOffset) * verticalAmplitude;
        beeRigidBody.velocity += tempSwayData;
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

    void Hover()
    {
        tempSwayData.x = Mathf.Sin(Time.realtimeSinceStartup * hoverSidewaysSpeed + randomOffset) * sidewaysHoverAmplitude;
        tempSwayData.z = Mathf.Cos(Time.realtimeSinceStartup * hoverSidewaysSpeed + randomOffset) * sidewaysHoverAmplitude;
        tempSwayData.y = Mathf.Sin(Time.realtimeSinceStartup * verticalSwaySpeed + randomOffset) * verticalHoverAmplitude;
        beeRigidBody.velocity = tempSwayData;

        targetObject.GetComponent<Plant>().AssignBee(this);
        targetObject.GetComponent<Plant>().inRange = true;
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
            Debug.Log(memory.Count);
            Debug.Log(memory.Contains(obj.GetComponent<Plant>()));
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
        Debug.Log("Enqueue" + p);
        memory.Enqueue(p);
        memoryTimes.Enqueue(Time.realtimeSinceStartup);
    }
}
