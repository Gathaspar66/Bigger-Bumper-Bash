using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum DebugWindowEnum
{
    POINTS,
    SPEEDMULTIPLIER,
    LANEMULTIPLIER,
    CRATEMULTIPLIER,
    CRATEMULTIPLIERTIMER,
    SPEED,
    ACCELLERATING,
    BRAKING,
    TURNING
}

public class DebugWindow : MonoBehaviour
{
    public static DebugWindow instance;

    public TMP_Text textContainerWindow;
    public TMP_Text textContainerWindowValues;

    string textContainer;

    string points;
    string speedMultiplier;
    string laneMultiplier;
    string crateMultiplier;
    string crateMultiplierTimer;
    string speed;
    string accelerating;
    string braking;
    string turning;


    bool isMenuOn = false;
    public GameObject menuCanvas;


    private void Awake()
    {
        instance = this;
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleMenu();
        }
    }

    void ToggleMenu()
    {
        isMenuOn = !isMenuOn;
        menuCanvas.SetActive(isMenuOn);
    }

    public void UpdateDebugWindow(DebugWindowEnum textType, float value)
    {
        textContainer = value.ToString();
        UpdateDebugWindow(textType, textContainer);
    }

    public void UpdateDebugWindow(DebugWindowEnum textType, string textContainer)
    {
        switch (textType)
        {
            case DebugWindowEnum.POINTS:
                points = textContainer;
                break;
            case DebugWindowEnum.SPEEDMULTIPLIER:
                speedMultiplier = textContainer;
                break;
            case DebugWindowEnum.LANEMULTIPLIER:
                laneMultiplier = textContainer;
                break;
            case DebugWindowEnum.SPEED:
                speed = textContainer;
                break;
            case DebugWindowEnum.CRATEMULTIPLIER:
                crateMultiplier = textContainer;
                break;
            case DebugWindowEnum.CRATEMULTIPLIERTIMER:
                crateMultiplierTimer = textContainer;
                break;
            case DebugWindowEnum.ACCELLERATING:
                accelerating = textContainer;
                break;
            case DebugWindowEnum.BRAKING:
                braking = textContainer;
                break;
            case DebugWindowEnum.TURNING:
                turning = textContainer;
                break;
        }

        textContainerWindow.text = "points:    " +
                                   "\nspeed multiplier:    " +
                                   "\nspeed:    " +
                                   "\nlane multiplier:    " +
                                   "\ncrate multiplier:    " +
                                   "\ncrate multiplier timer:    " +
                                   "\naccelerating:    " +
                                   "\nbraking:    " +
                                   "\nturning:    ";

        textContainerWindowValues.text = points + "\n" +
                                         speedMultiplier + "\n" +
                                         speed + "\n" +
                                         laneMultiplier + "\n" +
                                         crateMultiplier + "\n" +
                                         crateMultiplierTimer + "\n" +
                                         accelerating + "\n" +
                                         braking + "\n" +
                                         turning;
    }
}