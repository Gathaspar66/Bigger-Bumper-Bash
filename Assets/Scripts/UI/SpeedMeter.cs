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
        UpdateSpeed();
    }

    void UpdateSpeed()
    {
        UpdateSpeedBar();
    }

    void UpdateSpeedBar()
    {
        carSpeed = car.GetComponent<Rigidbody>().velocity.z;
        text.text = "x1";
        currentSpeedBarGreenSize = 0;
        currentSpeedBarOrangeSize = 0;
        maxSpeed = false;

        if (carSpeed >= breakpoints[0])
        {
            currentSpeedBarGreenSize = Mathf.Clamp((carSpeed - breakpoints[0]) / (breakpoints[1]- breakpoints[0]) * speedBarGreenSize, 0, speedBarGreenSize);
            if (carSpeed >= breakpoints[1])
            {
                text.text = "x2";
                currentSpeedBarOrangeSize = Mathf.Clamp((carSpeed - breakpoints[1]) / (breakpoints[2]- breakpoints[1]) * speedBarOrangeSize, 0, speedBarOrangeSize);
                if(carSpeed >= breakpoints[2])
                {
                    text.text = "x3";
                    maxSpeed = true;
                }
            }
        }
        speedBarGreen.sizeDelta = new Vector2(speedBarGreen.sizeDelta.x, currentSpeedBarGreenSize);
        speedBarOrange.sizeDelta = new Vector2(speedBarOrange.sizeDelta.x, currentSpeedBarOrangeSize);
        redBgBig.SetActive(maxSpeed);
    }

    void SetupBreakpoints()
    {
        breakpoints = car.GetComponent<PlayerSteering>().GetSpeedBreakpoints();
    }
}
