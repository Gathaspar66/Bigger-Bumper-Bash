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
    
    private void Awake()
    {
        instance = this;
    }

    public void UpdateDebugWindow(DebugWindowEnum textType, float value)
    {
        textContainer = value.ToString();
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
            case DebugWindowEnum.CRATEMULTIPLIER:
                crateMultiplier = textContainer;
                break;
            case DebugWindowEnum.CRATEMULTIPLIERTIMER:
                crateMultiplierTimer = textContainer;
                break;
        }
        textContainerWindow.text = "points:    " +
                                    "\nspeed multiplier:    " +
                                    "\nlane multiplier:    " +
                                    "\ncrate multiplier:    " +
                                    "\ncrate multiplier timer:    ";

        textContainerWindowValues.text =    points + "\n" +
                                            speedMultiplier + "\n" +
                                            laneMultiplier + "\n" +
                                            crateMultiplier + "\n" +
                                            crateMultiplierTimer;
    }
}
