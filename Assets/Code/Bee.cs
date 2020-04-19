﻿using Assets.Code;
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

    void Start()
    {
        //initiallizations when bee spawns
        hasPollinated = false;
        pollinationCount = 0;
        tempSwayData = Vector3.zero;
        randomOffset = UnityEngine.Random.Range(0f,2*Mathf.PI);
        player = GameObject.FindGameObjectWithTag("Player");

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
    }

    private void MoveToRandom()
    {
        targetTimer += Time.deltaTime;

        Debug.Log("Timer: " + targetTimer);
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
        Debug.Log("new random target");
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
        tempSwayData.x = Mathf.Cos(Time.realtimeSinceStartup * sidewaysSwaySpeed *randomOffset) * horizontalAmplitude;
        tempSwayData.y = Mathf.Sin(Time.realtimeSinceStartup * verticalSwaySpeed * randomOffset) * verticalAmplitude;
        beeRigidBody.velocity += tempSwayData;

        if (Vector3.Distance(target, transform.position)<targetRange)
        {
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
        foreach (GameObject flower in objects)
        {
            tempDistance = Vector3.Distance(flower.transform.position, transform.position);
            if (tempDistance < leastDistance)
            {
                leastDistance = tempDistance;
                closestObject = flower;
            }
        }

        return closestObject;
    }
}
