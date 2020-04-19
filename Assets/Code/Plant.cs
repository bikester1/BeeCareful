﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour
{

    public bool isPollinated;
    public bool inRange;
    private float pollinationTimer;
    public int timeToPollinate;
    public int cooldown;
    private float cooldownTimer;
    public Bee currentBee; //PASS CLOSEST BEE TO PLANT


    // Start is called before the first frame update
    void Start()
    {
        inRange = false;
        isPollinated = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (inRange)
        {
            if (isPollinated)
            {
                Debug.Log("Already pollinated");
                currentBee.changeBehaviorType(1);
            } else
            {
                pollinationTimer += Time.deltaTime;
                if (pollinationTimer >= timeToPollinate)
                {
                    Debug.Log("Pollinated and switched behavior");
                    isPollinated = true;
                    currentBee.changeBehaviorType(1);
                    currentBee.pollinated();
                }
            }
            currentBee.AddPlantToMemory(this);
        }

        if (cooldownTimer>=cooldown)
        {
            isPollinated = false;
        }
    }

    public void AssignBee(Bee x)
    {
        currentBee = x;
    }
}
