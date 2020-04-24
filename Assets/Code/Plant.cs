using Assets.Code;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine.UI;
using UnityEngine;

public class Plant : MonoBehaviour, Debuggable
{
    public enum growthState
    {
        Seed,
        Sapling,
        Mature
    };

    public bool isPollinated;
    public bool beeInRange;
    private float pollinationTimer;
    public int timeToPollinate;
    public int cooldown;
    private float cooldownTimer;
    public Bee currentBee; //PASS CLOSEST BEE TO PLANT
    public bool occupied;

    public float saplingAge;
    public float maturityAge;
    public float seedHydrationRequired;
    public float saplingHydrationRequired;
    public float matureHydrationRequired;

    public Color seedColor;
    public Color saplingColor;
    public Color matureColor;

    private float age;
    private float hydration;
    private growthState stage;


    // Start is called before the first frame update
    void Start()
    {
        beeInRange = false;
        isPollinated = false;
        occupied = false;

        hydration = 2;
    }

    // Update is called once per frame
    void Update()
    {
        if (!enabled) return;

        if (beeInRange)
        {
            if (isPollinated)
            {
                // Bee finds out no necture
                currentBee.changeBehaviorType(1);
                currentBee.AddPlantToMemory(this);
                beeInRange = false;
            } else
            {
                pollinationTimer += Time.deltaTime;
                if (pollinationTimer >= timeToPollinate)
                {
                    // bee finished getting necture
                    isPollinated = true;
                    occupied = false;
                    pollinationTimer = 0;
                    currentBee.changeBehaviorType(1);
                    currentBee.AddPlantToMemory(this);
                    beeInRange = false;
                    currentBee.pollinated();
                    currentBee = null;
                }
            }

        }

        if (cooldownTimer >= cooldown)
        {
            isPollinated = false;
            cooldownTimer = 0;
        }
        if(isPollinated)cooldownTimer += Time.deltaTime;

        PlantGrowth();
    }

    private void PlantGrowth()
    {
        if (age < saplingAge) stage = growthState.Seed;
        else if (age < maturityAge) stage = growthState.Sapling;
        else stage = growthState.Mature;


        switch (stage)
        {
            case growthState.Seed:
                transform.localScale = Vector3.zero;
                if (hydration > seedHydrationRequired) age += Time.deltaTime;
                break;
            case growthState.Sapling:
                SetGrowthVisually((age - saplingAge)/maturityAge);
                if (hydration > saplingHydrationRequired) age += Time.deltaTime;
                break;
            case growthState.Mature:
                UpdateMature();
                if (hydration > matureHydrationRequired) age += Time.deltaTime;
                break;
        }



    }

    public void AssignBee(Bee x)
    {
        currentBee = x;
    }

    private void SetGrowthVisually(float percentGrown)
    {
        GetComponent<Renderer>().material.color = Color.Lerp(saplingColor, matureColor, percentGrown);
        transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, percentGrown);
    }

    
    private void UpdateMature()
    {

    }

    //private float age;
    //private float hydration;
    //private growthState stage;

    public string GetDebugInfo()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("plant Object:" + this.GetHashCode());
        sb.Append("\nPollinated:" + isPollinated);
        sb.Append("\nIn Range:" + beeInRange);
        sb.Append("\nOccupation:" + occupied);
        sb.Append("\nTime To Depollinate:" + (cooldown - cooldownTimer));
        sb.Append("\nTime To Pollinate:" + (timeToPollinate - pollinationTimer));
        sb.Append("\nsaplingAge: " + saplingAge);
        sb.Append("\nmaturityAge: " + maturityAge);
        sb.Append("\nseedHydrationRequired: " + seedHydrationRequired);
        sb.Append("\nsaplingHydrationRequired: " + saplingHydrationRequired);
        sb.Append("\nmatureHydrationRequired: " + matureHydrationRequired);
        sb.Append("\nage: " + age);
        sb.Append("\nhydration: " + hydration);
        sb.Append("\nstage: "+ stage);
        sb.Append("\nEnabled: " + enabled);
        sb.Append("\n");
        return sb.ToString();
    }
}
