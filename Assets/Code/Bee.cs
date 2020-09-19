﻿using Assets.Code;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class Bee : Entity, Debuggable
{

    public enum behaviorState 
    {
        Search,
        Homing,
        Pollinating,
        Attacking
    };
    private behaviorState behavior;

    [SerializeField] private Rigidbody beeRigidbody;
    #region Search Variables
    private float randomOffset, directionTimer;
    [SerializeField] private float detectionRadius, searchSpeed, timeToChangeDirection, veritcalSwaySpeed, verticalAmplitude;
    private Vector3 targetAngle, swayVector;
    #endregion

    #region Homing Variables
    [SerializeField] private float homingSpeed;
    #endregion

    #region Pollinating Variables
    [SerializeField] private int pollinationCount;
    #endregion

    #region Attacking Variables

    #endregion

    private Hive homeHive;
    public Hive HomeHive { get => homeHive; set => homeHive = value; }

    // Start is called before the first frame update
    void Start()
    {
        behavior = behaviorState.Search;
        swayVector = Vector3.zero;
        randomOffset = UnityEngine.Random.Range(0f, 2 * Mathf.PI);

    }

    // Update is called once per frame
    void Update()
    {
        switch (behavior)
        {
            case behaviorState.Search:
                UpdateSearch();
                break;
            case behaviorState.Homing:
                UpdateHoming();
                break;
            case behaviorState.Pollinating:
                UpdatePollinating();
                break;
            case behaviorState.Attacking:
                UpdateAttacking();
                break;
        }
    }

    #region Search Functions
    void StateToSearch()
    {
        behavior = behaviorState.Search;
    }

    void UpdateSearch()
    {
        directionTimer += Time.deltaTime;
        if (directionTimer > timeToChangeDirection)
        {
            ChangeDirection();
            directionTimer -= timeToChangeDirection;
        }

        Vector3 previousLook = beeRigidbody.transform.rotation * Vector3.forward;
        beeRigidbody.transform.rotation = Quaternion.LookRotation(
            Vector3.Lerp(
                previousLook,
                targetAngle,
                Time.deltaTime * .5f
            ),
            Vector3.up
        );
        
        beeRigidbody.velocity = transform.forward.normalized * searchSpeed;
        swayVector.y = Mathf.Sin(Time.realtimeSinceStartup * veritcalSwaySpeed * randomOffset) * verticalAmplitude;
        beeRigidbody.velocity += swayVector;

    }

    void ChangeDirection()
    {
        float newHeading = UnityEngine.Random.Range(30f, 330f);
        targetAngle = Quaternion.AngleAxis(newHeading, Vector3.up) * Vector3.forward;
        
    }

    #endregion

    #region Homing
    void StateToHoming()
    {
        behavior = behaviorState.Homing;
    }

    void UpdateHoming() //The accuracy of Bee Homing Flights changes depending on closeness to bee hive, implement this
    {
        beeRigidbody.transform.LookAt(homeHive.transform.position);
        beeRigidbody.velocity = transform.forward.normalized * homingSpeed;
        swayVector.y = Mathf.Sin(Time.realtimeSinceStartup * veritcalSwaySpeed * randomOffset) * verticalAmplitude;
        beeRigidbody.velocity += swayVector;
    }
    #endregion

    #region Pollinating Functions
    void StateToPollinating()
    {
        behavior = behaviorState.Pollinating;
    }

    void UpdatePollinating()
    {
        
    }
    #endregion

    #region Attacking Functions
    void StateToAttacking()
    {
        behavior = behaviorState.Attacking;//
    }

    void UpdateAttacking()
    {

    }
    #endregion


    public string GetDebugInfo()
    {
        throw new NotImplementedException();
    }
}
