using Assets.Code;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class Plant : MonoBehaviour, Debuggable
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
                currentBee.changeBehaviorType(1);
                currentBee.AddPlantToMemory(this);
                inRange = false;
            } else
            {
                pollinationTimer += Time.deltaTime;
                if (pollinationTimer >= timeToPollinate)
                {
                    isPollinated = true;
                    pollinationTimer = 0;
                    currentBee.changeBehaviorType(1);
                    currentBee.AddPlantToMemory(this);
                    inRange = false;
                    currentBee.pollinated();
                }
            }

        }

        if (cooldownTimer >= cooldown)
        {
            isPollinated = false;
            cooldownTimer = 0;
        }
        if(!isPollinated)cooldownTimer += Time.deltaTime;
    }

    public void AssignBee(Bee x)
    {
        currentBee = x;
    }

    public string GetDebugInfo()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("plant Object:" + this.GetHashCode());
        sb.Append("\nPollinated:" + isPollinated);
        sb.Append("\nIn Range:" + inRange);
        sb.Append("\nTime To Depollinate:" + (cooldown - cooldownTimer));
        sb.Append("\nTime To Pollinate:" + (timeToPollinate - pollinationTimer));
        return sb.ToString();
    }
}
