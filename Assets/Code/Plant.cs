using Assets.Code;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine.UI;
using UnityEngine;
using Unity.Profiling;

public class Plant : MonoBehaviour, Debuggable, Placeable
{
    public enum growthStates
    {
        Seed,
        Sapling,
        Mature,
        Placing
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
    private growthStates growthState;

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
        StateToSeed();
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceUpdate += Time.deltaTime;

        if(timeSinceUpdate >= updateRate / growthRateMultiplier)
        {
            // preserves frame errors by removing a second of time vs setting to 0
            timeSinceUpdate -= updateRate / growthRateMultiplier;
            ManageStates();
        }
    }

    #region States
    private void ManageStates()
    {
        switch (growthState)
        {
            case growthStates.Seed:
                UpdateSeed();
                break;

            case growthStates.Sapling:
                UpdateSapling();
                break;

            case growthStates.Mature:
                UpdateMature();
                break;

            case growthStates.Placing:
                UpdatePlacing();
                break;
        }
    }

    public void StateToSeed()
    {
        growthState = growthStates.Seed;
    }
    private void UpdateSeed()
    {
        transform.localScale = Vector3.zero;
        if (hydration >= updateRate) 
        { 
            age += updateRate;
            hydration -= updateRate;
        }

        if (age > saplingAge) StateToSapling();
    }

    public void StateToSapling()
    {
        growthState = growthStates.Sapling;
    }
    private void UpdateSapling()
    {
        if (hydration >= updateRate)
        {
            age += updateRate;
            hydration -= updateRate;
        }

        SetGrowthVisually((age - saplingAge) / (maturityAge - saplingAge));

        if (age > maturityAge) StateToMature();
    }

    public void StateToMature()
    {
        growthState = growthStates.Mature;
    }
    private void UpdateMature()
    {
        if (hydration >= updateRate)
        {
            pollen += updateRate;
            hydration -= updateRate;
        }    
    }

    public void StateToPlacing()
    {
        growthState = growthStates.Placing;
        SetGrowthVisually(1);
    }
    private void UpdatePlacing()
    {

    }
    #endregion

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
        sb.Append("\nstage: "+ growthState);
        sb.Append("\nEnabled: " + enabled);
        sb.Append("\n");
        return sb.ToString();
    }

    public void Place()
    {
        StateToSeed();
    }
}
