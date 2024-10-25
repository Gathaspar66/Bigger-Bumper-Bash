using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SpeedMeter : MonoBehaviour
{
    public RectTransform speedBarGreen;
    public RectTransform speedBarOrange;
    public RectTransform speedBarRed;
    public TMP_Text text;

    public GameObject redBgBig;

    float speedBarGreenSize = 100;
    float speedBarOrangeSize = 100;

    float currentSpeedBarGreenSize;
    float currentSpeedBarOrangeSize;
    float carSpeed = 0;

    int speedMultiplier = 1;

    bool maxSpeed;

    GameObject car;

    List<float> breakpoints;

    // Start is called before the first frame update
    void Start()
    {
        car = PlayerManager.instance.GetPlayerInstance();
        SetupBreakpoints();
    }

    void Update()
    {
        CalculateSpeed();
    }

    void CalculateSpeed()
    {
        carSpeed = car.GetComponent<Rigidbody>().velocity.z;
        DebugWindow.instance.UpdateDebugWindow(DebugWindowEnum.SPEED, car.GetComponent<Rigidbody>().velocity.z);
        currentSpeedBarGreenSize = 0;
        currentSpeedBarOrangeSize = 0;
        maxSpeed = false;
        speedMultiplier = 1;

        if (carSpeed >= breakpoints[0])
        {
            currentSpeedBarGreenSize = Mathf.Clamp((carSpeed - breakpoints[0]) / (breakpoints[1]- breakpoints[0]) * speedBarGreenSize, 0, speedBarGreenSize);
            if (carSpeed >= breakpoints[1])
            {
                speedMultiplier = 2;
                currentSpeedBarOrangeSize = Mathf.Clamp((carSpeed - breakpoints[1]) / (breakpoints[2]- breakpoints[1]) * speedBarOrangeSize, 0, speedBarOrangeSize);
                if(carSpeed >= breakpoints[2])
                {
                    speedMultiplier = 3;
                    maxSpeed = true;
                }
            }
        }

        text.text = "x" + speedMultiplier.ToString();
        speedBarGreen.sizeDelta = new Vector2(speedBarGreen.sizeDelta.x, currentSpeedBarGreenSize);
        speedBarOrange.sizeDelta = new Vector2(speedBarOrange.sizeDelta.x, currentSpeedBarOrangeSize);
        redBgBig.SetActive(maxSpeed);

        NotifyPointsManager(speedMultiplier);
    }

    void NotifyPointsManager(int speedMultiplier)
    {
        PointsManager.instance.UpdateSpeedMultiplier(speedMultiplier);
    }

    void SetupBreakpoints()
    {
        breakpoints = car.GetComponent<PlayerSteering>().GetSpeedBreakpoints();
    }
}
