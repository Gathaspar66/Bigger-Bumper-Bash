using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpeedMeter : MonoBehaviour
{
    private readonly float speedBarGreenSize = 100;
    private readonly float speedBarOrangeSize = 100;
    private float currentSpeedBarGreenSize;
    private float currentSpeedBarOrangeSize;
    private float carSpeed = 0;
    private int speedMultiplier = 1;
    private bool maxSpeed;
    private GameObject car;
    private List<float> breakpoints;

    public Image speedFillImage;
    public GameObject superSpeed;

    public List<Sprite> speedMultiplierList = new();
    public Image multiplierValueImage;

    private void Start()
    {
        GeneralSetup();
        SetupBreakpoints();

        SetFill(0f);
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
        car = PlayerManager.instance.GetPlayerInstance();
    }

    private void CalculateSpeed()
    {
        float fill = 0f;

        carSpeed = car.GetComponent<Rigidbody>().velocity.z;
        currentSpeedBarGreenSize = 0;
        currentSpeedBarOrangeSize = 0;
        maxSpeed = false;
        speedMultiplier = 1;

        if (carSpeed >= breakpoints[0])
        {
            fill = Mathf.Clamp01((carSpeed - breakpoints[0]) / (breakpoints[2] - breakpoints[0]));
            currentSpeedBarGreenSize =
                Mathf.Clamp((carSpeed - breakpoints[0]) / (breakpoints[1] - breakpoints[0]) * speedBarGreenSize, 0,
                    speedBarGreenSize);
            if (carSpeed >= breakpoints[1])
            {
                speedMultiplier = 2;
                currentSpeedBarOrangeSize =
                    Mathf.Clamp((carSpeed - breakpoints[1]) / (breakpoints[2] - breakpoints[1]) * speedBarOrangeSize, 0,
                        speedBarOrangeSize);
                if (carSpeed >= breakpoints[2])
                {
                    speedMultiplier = 3;
                    maxSpeed = true;
                }
            }
        }

        SetFill(fill);

        UpdateSpeedMultiplierVisual(speedMultiplier);


        // redBgBig.SetActive(maxSpeed);
        superSpeed.SetActive(maxSpeed);
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
        breakpoints = car.GetComponent<PlayerSteering>().GetSpeedBreakpoints();
    }
}