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

    private bool occupied;

    private float timeSinceUpdate;
    private float updateRate = 1.0f;
    // Only use this when debugging
    private float growthRateMultiplier = 10.0f;

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
        occupied = false;

        saplingAge = 60 * 1.0f;
        maturityAge = 60 * 10.0f;
        
        age = 0;
        hydration = 1000;
        pollen = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (!enabled) return;

        timeSinceUpdate += Time.deltaTime;

        if(timeSinceUpdate >= updateRate / growthRateMultiplier)
        {
            // preserves frame errors by removing a second of time vs setting to 0
            timeSinceUpdate -= updateRate / growthRateMultiplier;
            ManageGrowth();
        }
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
                GrowSapling();
                break;
            case growthState.Mature:
                GrowMature();
                break;
        }
    }

    private void GrowSeed()
    {
        transform.localScale = Vector3.zero;
        if (hydration > updateRate) 
        { 
            age += updateRate;
            hydration -= updateRate;
        }
    }

    private void GrowSapling()
    {
        if (hydration > updateRate)
        {
            age += updateRate;
            hydration -= updateRate;
        }

        SetGrowthVisually((age - saplingAge) / (maturityAge - saplingAge));
    }

    private void GrowMature()
    {
        if (hydration > updateRate)
        {
            pollen += updateRate;
            hydration -= updateRate;
        }    
    }

    private void SetGrowthVisually(float percentGrown)
    {
        GetComponent<Renderer>().material.color = Color.Lerp(saplingColor, matureColor, percentGrown);
        transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, percentGrown);
    }

    /// <summary>
    /// adds hydration to plant and returns the amount of hydration to remove from hydration source.
    /// </summary>
    /// <param name="hydrationAmount"></param>
    /// <returns></returns>
   public float WaterPlant(float hydrationAmount)
    {
        hydration += hydrationAmount;
        return hydrationAmount;
    }

    //private float age;
    //private float hydration;
    //private growthState stage;

    public string GetDebugInfo()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append("plant Object:" + this.GetHashCode());
        sb.Append("\nsaplingAge: " + saplingAge);
        sb.Append("\nmaturityAge: " + maturityAge);
        sb.Append("\nseedHydrationRequired: " + seedHydrationRequired);
        sb.Append("\nsaplingHydrationRequired: " + saplingHydrationRequired);
        sb.Append("\nmatureHydrationRequired: " + matureHydrationRequired);
        sb.Append("\nage: " + age);
        sb.Append("\nPollen:" + pollen);
        sb.Append("\nhydration: " + hydration);
        sb.Append("\nstage: "+ stage);
        sb.Append("\nEnabled: " + enabled);
        sb.Append("\n");
        return sb.ToString();
    }
}
