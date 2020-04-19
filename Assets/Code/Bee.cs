﻿using Assets.Code;
using UnityEngine;

public class Bee : Entity
{

    //General
    public Rigidbody beeRigidBody;
    public float forwardSpeed;
    private int behaviorType;
    public float detectionRadius;
    

    //Used for determining bee's next target
    private bool hasPollinated;
    public int maxPollinationCount;
    private int pollinationCount;
    private Vector3 target;

    //Configures how much bee moves around.
    public float verticalSwaySpeed; //How fast bee sways up and down
    public float sidewaysSwaySpeed; //How fast bee sways side to side
    public float verticalAmplitude; //How far up and down bee moves
    public float horizontalAmplitude; //How far side to side bee moves
    private Vector3 tempSwayData; //Used to store sway data

    //Needed for Random Movement
    private float targetTimer;
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

    void Start()
    {
        //initiallizations when bee spawns
        hasPollinated = false;
        pollinationCount = 0;
        tempSwayData = Vector3.zero;
        randomOffset = Random.Range(0f,2*Mathf.PI);


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
                target = findTargetPosition();
                MoveToTarget();
                break;

            case 3: //Bee is within range of target
                CloseToTarget();
                break;

            case 4:
                Hover();
                break;
        }
    }

    //Determines bee's target
    Vector3 findTargetPosition()
    {
        //Bee can pollinate only so many times before it must return to hive.
        if (pollinationCount < maxPollinationCount)
        {
            GameObject[] flowers = GameObject.FindGameObjectsWithTag("Plant");
            GameObject closestFlower = null;
            float leastDistance = Mathf.Infinity;
            float tempDistance;
            foreach(GameObject flower in flowers)
            {
                tempDistance = Vector3.Distance(flower.transform.position, transform.position);
                if (tempDistance < leastDistance)
                {
                    leastDistance = tempDistance;
                    closestFlower = flower;
                }
            }


            return closestFlower.transform.position + Vector3.up * heightDisplacement;


        } else
        {
            return GameObject.Find("Hive").transform.position;
        }
        
    }

    void LookForTarget()
    {
        Debug.Log(Vector3.Distance(findTargetPosition(), transform.position));
        if (Vector3.Distance(findTargetPosition(), transform.position) < detectionRadius)
        {
            Debug.Log("found target");
            behaviorType = 2;
            findTargetPosition();
        } else
        {
            targetTimer += Time.deltaTime;
            if (targetTimer >= findNewTarget) {
                RandomTarget();
                targetTimer = 0;
            }
            MoveToTarget();
        }
    }

    void RandomTarget ()
    {
        Debug.Log("new random target");
        float myX = gameObject.transform.position.x;
        float myZ = gameObject.transform.position.z;

        float xPos = myX + Random.Range(myX - 50, myX + 50);
        float zPos = myZ + Random.Range(myZ - 50, myZ + 50);

        target = new Vector3(xPos, gameObject.transform.position.y, zPos);
    }

    void MoveToTarget()
    {
        beeRigidBody.transform.LookAt(target);
        beeRigidBody.velocity = transform.forward.normalized * forwardSpeed;
        tempSwayData.x = Mathf.Cos(Time.realtimeSinceStartup * sidewaysSwaySpeed *randomOffset) * horizontalAmplitude;
        tempSwayData.y = Mathf.Sin(Time.realtimeSinceStartup * verticalSwaySpeed * randomOffset) * verticalAmplitude;
        beeRigidBody.velocity += tempSwayData;

        if (Vector3.Distance(findTargetPosition(), transform.position)<targetRange)
        {
            behaviorType = 3;
        }
    }

    void CloseToTarget()
    {
        beeRigidBody.transform.LookAt(target);
        beeRigidBody.velocity = transform.forward.normalized * forwardSpeed;
        tempSwayData.y = Mathf.Sin(Time.realtimeSinceStartup * verticalSwaySpeed* randomOffset) * verticalAmplitude;
        beeRigidBody.velocity += tempSwayData;
        float upperX = target.x + .1f;
        float lowerX = target.x - .1f;
        float upperZ = target.z + .1f;
        float lowerZ = target.z - .1f;
        if (transform.position.x <= upperX && transform.position.x >= lowerX && transform.position.z <= upperZ && transform.position.z >= lowerZ) behaviorType = 4;
    }

    void Hover()
    {
        tempSwayData.x = Mathf.Sin(Time.realtimeSinceStartup * hoverSidewaysSpeed * randomOffset) * sidewaysHoverAmplitude;
        tempSwayData.z = Mathf.Cos(Time.realtimeSinceStartup * hoverSidewaysSpeed * randomOffset) * sidewaysHoverAmplitude;
        tempSwayData.y = Mathf.Sin(Time.realtimeSinceStartup * verticalSwaySpeed * randomOffset) * verticalHoverAmplitude;
        beeRigidBody.velocity = tempSwayData;
    }

<<<<<<< HEAD
    public void OnCollisionEnter(Collision collision)
    {
        Debug.Log("BeeCollision");
        ContactPoint[] contacts = new ContactPoint[collision.contactCount];
        collision.GetContacts(contacts);
=======
>>>>>>> 4147037c244b3f42ad37bcc5478d03c08e60aea4

        for(int i = 0; i < collision.contactCount; i++)
        {
            Debug.Log(contacts[i].otherCollider.transform.tag);
            if (contacts[i].otherCollider.transform.tag.Contains("Player"))
            {
                Debug.Log("Destroy all humans");
                Destroy(this.gameObject);
            }
        }
    }
}
