using Assets.Code;
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

    [SerializeField] private Rigidbody beeRigidBody;

    #region Search Variables
    [SerializeField] private float detectionRadius, searchSpeed, directionTimer, timeToChangeDirection;
    private Vector3 randomTarget;
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
            directionTimer = 0;
        }
        beeRigidBody.transform.LookAt(randomTarget);
        beeRigidBody.velocity = transform.forward.normalized * searchSpeed;

    }

    void ChangeDirection()
    {

        float myX = transform.position.x;
        float myZ = transform.position.z;

        float xPos = UnityEngine.Random.Range(myX - 50, myX + 50);
        float zPos = UnityEngine.Random.Range(myZ - 50, myZ + 50);

        randomTarget = new Vector3(xPos, gameObject.transform.position.y, zPos);
        
    }

    #endregion

    #region Homing
    void StateToHoming()
    {
        behavior = behaviorState.Homing;
    }

    void UpdateHoming() //The accuracy of Bee Homing Flights changes depending on closeness to bee hive, implement this
    {
        beeRigidBody.transform.LookAt(homeHive.transform.position);
        beeRigidBody.velocity = transform.forward.normalized * homingSpeed;
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
