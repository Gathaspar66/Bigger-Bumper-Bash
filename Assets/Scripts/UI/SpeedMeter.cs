using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeedMeter : MonoBehaviour
{
    //WARNING
    //Speedometer has been set up for linear, hard set car stat values (6 set segments)!
    //if curves/varied values for levels are set, speedometer needs adaptation for dynamic
    //values (e.g. level speed 6, but one car is 28 other is 30, SO change to contain
    //level, and fill bar calculate from speed, not as currently, from segment)
    private float carSpeed = 0;

    private int speedMultiplier = 1;
    private bool maxSpeedAchieved;
    private float maxSpeed;
    private float maxBarFill;
    private GameObject car;
    private List<float> breakpoints = new();
    private List<float> meterBreakpoints = new();

    public Image speedFillImage;
    public GameObject superSpeed;

    public List<Sprite> speedMultiplierList;
    public Image multiplierValueImage;

    public List<GameObject> speedMeterSegments = new();

    private void Start()
    {
        GeneralSetup();
    }

    private void Update()
    {
        CalculateSpeed();
    }

    private void UpdateSpeedMultiplierVisual(int value)
    {
        multiplierValueImage.sprite = speedMultiplierList[value - 1];
    }

    private void GeneralSetup()
    {
        SetupBreakpoints();
        car = PlayerManager.instance.GetPlayerInstance();
        maxSpeed = PlayerManager.instance.selectedCarData.maxForwardVelocity;
        for (int i = 0; i < Mathf.Clamp((maxSpeed - 10) / 3, 0, 6); i++)
        {
            speedMeterSegments[i].SetActive(true);
        }
        // print("max speed " + maxSpeed);
        // print("segments count " + (int)Mathf.Clamp((maxSpeed - 10) / 3, 0, 6));
        maxBarFill = meterBreakpoints[(int)Mathf.Clamp((maxSpeed - 10) / 3, 0, 6)];
    }

    private void CalculateSpeed()
    {
        float fill = 0f;

        carSpeed = car.GetComponent<Rigidbody>().velocity.z;
        maxSpeedAchieved = false;
        speedMultiplier = 1;

        if (carSpeed >= breakpoints[0])
        {
            //print(" speed " + carSpeed + " maxspeed " + maxSpeed + " maxbarfill " + maxBarFill);
            fill = Mathf.Clamp01((carSpeed - 10) / (maxSpeed - 10));
            //print("fill " + fill);
            fill *= maxBarFill;
            // print("fill " + fill);

            if (carSpeed >= breakpoints[2])
            {
                speedMultiplier = 2;
                if (carSpeed >= breakpoints[4])
                {
                    speedMultiplier = 3;
                    if (carSpeed >= breakpoints[6])
                    {
                        speedMultiplier = 4;
                        maxSpeedAchieved = true;
                    }
                }
            }
        }
        SetFill(fill);

        UpdateSpeedMultiplierVisual(speedMultiplier);

        superSpeed.SetActive(maxSpeedAchieved);
        NotifyPointsManager(speedMultiplier);
    }

    private void SetFill(float fillAmount)
    {
        speedFillImage.fillAmount = fillAmount;
    }

    private void NotifyPointsManager(int speedMultiplier)
    {
        PointsManager.instance.UpdateSpeedMultiplier(speedMultiplier);
    }

    private void SetupBreakpoints()
    {
        breakpoints = new List<float> { 10, 13, 16, 19, 22, 25, 28 };
        //NOT (10 + n*3) because speedometer's graphic is NOT evenly divided
        meterBreakpoints = new List<float> { 0, 0.24f, 0.38f, 0.53f, 0.66f, 0.82f, 1 };
    }
}