using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointsManager : MonoBehaviour
{
    public static PointsManager instance;
    public GameObject car;

    public float speedPointMultiplier = 1;
    public float lanePointMultiplier = 1;
    public float cratePointMultiplier = 1;
    public float cratePointMultiplierMax = 3;
    public float cratePointMultiplierTimer = 0;
    public float cratePointMultiplierTimerMax = 5;

    float lastCarLocationZ;
    float currentCarLocationZ;
    float pointsMultiplier = 0;

    public float points = 0;


    private void Awake()
    {
        instance = this;
    }


    public void Activate()
    {
        car = PlayerManager.instance.GetPlayerInstance();
        lastCarLocationZ = car.transform.position.z;
        DebugWindow.instance.UpdateDebugWindow(DebugWindowEnum.CRATEMULTIPLIER, cratePointMultiplier);
    }


    void Update()
    {
        CheckCrateMultiplierTimer();
        CalculatePoints();
    }

    void CalculatePoints()
    {
        currentCarLocationZ = car.transform.position.z;
        pointsMultiplier = speedPointMultiplier * lanePointMultiplier * cratePointMultiplier;
        UserInterfaceManager.instance.UpdatePointsMultiplayerDisplay(pointsMultiplier);
        points += (currentCarLocationZ - lastCarLocationZ) * pointsMultiplier;
        UserInterfaceManager.instance.UpdatePointsDisplay(points);
        lastCarLocationZ = car.transform.position.z;
        DebugWindow.instance.UpdateDebugWindow(DebugWindowEnum.POINTS, points);
    }

    public void AddPoints(float pointValue)
    {
        points += pointValue;

        CrateHit();
        DebugWindow.instance.UpdateDebugWindow(DebugWindowEnum.POINTS, points);
    }

    public void UpdateSpeedMultiplier(int value)
    {
        speedPointMultiplier = value;
        DebugWindow.instance.UpdateDebugWindow(DebugWindowEnum.SPEEDMULTIPLIER, speedPointMultiplier);
    }

    public void UpdateLaneMultiplier(int value)
    {
        lanePointMultiplier = value;
        DebugWindow.instance.UpdateDebugWindow(DebugWindowEnum.LANEMULTIPLIER, lanePointMultiplier);
    }

    public void CrateHit()
    {
        cratePointMultiplierTimer = cratePointMultiplierTimerMax;
        if (cratePointMultiplier != cratePointMultiplierMax) //if crate multiplier not max, add 1
        {
            cratePointMultiplier += 1;
            //if crate multiplier above max, set to max
            if (cratePointMultiplier > cratePointMultiplierMax) cratePointMultiplier = cratePointMultiplierMax;
        }

        DebugWindow.instance.UpdateDebugWindow(DebugWindowEnum.CRATEMULTIPLIER, cratePointMultiplier);
    }

    void CheckCrateMultiplierTimer()
    {
        if (cratePointMultiplier == 1) return;

        cratePointMultiplierTimer -= Time.deltaTime;
        if (cratePointMultiplierTimer < 0)
        {
            cratePointMultiplierTimer = cratePointMultiplierTimerMax;
            if (cratePointMultiplier > 1) cratePointMultiplier -= 1;
            DebugWindow.instance.UpdateDebugWindow(DebugWindowEnum.CRATEMULTIPLIER, cratePointMultiplier);
        }

        UserInterfaceManager.instance.UpdatePickupMultiplier((int)cratePointMultiplier);
        DebugWindow.instance.UpdateDebugWindow(DebugWindowEnum.CRATEMULTIPLIERTIMER, cratePointMultiplierTimer);
    }

    public void SavePlayerScore()
    {
        if (points <= PlayerPrefs.GetInt("highScore")) return;
        PlayerPrefs.SetInt("highScore", (int)points);
    }
}