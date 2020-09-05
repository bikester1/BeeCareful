using Assets.Code;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine.UI;
using UnityEngine;
using Unity.Profiling;

public class Plant : MonoBehaviour, Debuggable
{
    public enum growthState
    {
        Seed,
        Sapling,
        Mature
    };

    private bool pollinated;
    private bool beeInRange;
    private float pollinationTimer;
    private int timeToPollinate;
    private int cooldown;
    private float cooldownTimer;
    private bool occupied;

    private float saplingAge;
    private float maturityAge;
    private float seedHydrationRequired;
    private float saplingHydrationRequired;
    private float matureHydrationRequired;

    private Color seedColor;
    private Color saplingColor;
    private Color matureColor;

    private float age;
    private float hydration;
    private float pollen;
    private growthState stage;

    public bool IsOccupied { get => occupied; }
    public float Hydration { get => hydration; }
    public float Age { get => age; }
    public float Pollen { get => pollen; }


    // Start is called before the first frame update
    void Start()
    {
        beeInRange = false;
        pollinated = false;
        occupied = false;

        age = 0;
        hydration = 0;
        pollen = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (!enabled) return;


        if (cooldownTimer >= cooldown)
        {
            pollinated = false;
            cooldownTimer = 0;
        }
        if(pollinated)cooldownTimer += Time.deltaTime;

        ManageGrowth();
    }

    private void ManageGrowth()
    {
        if (age < saplingAge) stage = growthState.Seed;
        else if (age < maturityAge) stage = growthState.Sapling;
        else stage = growthState.Mature;


        switch (stage)
        {
            case growthState.Seed:
                GrowSeed();
                break;
            case growthState.Sapling:
                GrowSappling();
                break;
            case growthState.Mature:
                GrowMature();
                break;
        }
    }

    private void GrowSeed()
    {
        transform.localScale = Vector3.zero;
        if (hydration > seedHydrationRequired) age += Time.deltaTime;
    }

    private void GrowSappling()
    {
        SetGrowthVisually((age - saplingAge) / maturityAge);
        if (hydration > saplingHydrationRequired) age += Time.deltaTime;
    }

    private void GrowMature()
    {
        if (hydration > matureHydrationRequired) age += Time.deltaTime;
    }


    private void SetGrowthVisually(float percentGrown)
    {
        GetComponent<Renderer>().material.color = Color.Lerp(saplingColor, matureColor, percentGrown);
        transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, percentGrown);
    }

   

    //private float age;
    //private float hydration;
    //private growthState stage;

    public string GetDebugInfo()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("plant Object:" + this.GetHashCode());
        sb.Append("\nPollinated:" + pollinated);
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
