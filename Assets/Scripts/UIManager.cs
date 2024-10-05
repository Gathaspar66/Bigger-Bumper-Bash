using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public RectTransform speedBarGreen;
    public RectTransform speedBarOrange;
    public RectTransform speedBarRed;
    public TMP_Text text;

    GameObject car;

    List<float> breakpoints = new List<float>() { 10f, 20f };

    // Start is called before the first frame update
    void Start()
    {
        car = ControlsCameraChoice.instance.GetCar();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateSpeed();
    }
    
    void UpdateSpeed()
    {
        float carSpeed = car.GetComponent<Rigidbody>().velocity.z;
        UpdateSpeedBar(carSpeed);
    }

    void UpdateSpeedBar(float value)
    {
        text.text = "x1";
        speedBarGreen.sizeDelta = new Vector2(speedBarGreen.sizeDelta.x, value * 10);

        if (value > 10)
        {
            text.text = "x2";
            speedBarOrange.sizeDelta = new Vector2(speedBarOrange.sizeDelta.x, value * 10 - 100);
            if(value > 20)
            {
                text.text = "x3";
                speedBarRed.sizeDelta = new Vector2(speedBarRed.sizeDelta.x, value * 10 - 200);
            }
            else
            {
                speedBarRed.sizeDelta = new Vector2(speedBarRed.sizeDelta.x, 0);
            }
        }
        else
        {
            speedBarOrange.sizeDelta = new Vector2(speedBarOrange.sizeDelta.x, 0);
        }
    }
}
