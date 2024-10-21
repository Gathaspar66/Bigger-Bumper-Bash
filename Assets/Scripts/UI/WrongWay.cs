using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WrongWay : MonoBehaviour
{
    GameObject car;
    bool hardLane = false;
    bool bonusActive = false;
    public float hardLaneSwitchTimerCurrent = 0;
    float hardLaneSwitchTimerMax = 1;

    public TMP_Text text, textbg;
    public GameObject bonusText;
    string textToInput = "Wrong Way";

    // Start is called before the first frame update
    void Start()
    {
        car = PlayerManager.instance.GetPlayerInstance();
    }

    // Update is called once per frame
    void Update()
    {
        car = PlayerManager.instance.GetPlayerInstance();
        CheckLanes();
        HardLaneCheck();
    }

    void CheckLanes()
    {
        if (car.transform.position.x < 0)
        {
            EnterHardLane();
        }
        else
        {
            LeaveHardLane();
        }
    }

    void EnterHardLane()
    {
        hardLane = true;
    }
    
    void LeaveHardLane()
    {
        hardLane = false;
        hardLaneSwitchTimerCurrent = 0;
        text.alpha = 0;
        textbg.alpha = 0;
        ActivateBonus(false);
    }

    void HardLaneCheck()
    {
        if (!hardLane) return;
        hardLaneSwitchTimerCurrent += Time.deltaTime;
        hardLaneSwitchTimerCurrent = Mathf.Clamp(hardLaneSwitchTimerCurrent, 0, hardLaneSwitchTimerMax);
        HardLaneVisualUpdate();
    }

    void HardLaneVisualUpdate()
    {
        if (bonusActive) return;
        float alpha = Mathf.Clamp(hardLaneSwitchTimerCurrent * 4, 0, 1);
        text.alpha = alpha;
        textbg.alpha = alpha;

        int lettersCount = (int)(hardLaneSwitchTimerCurrent / hardLaneSwitchTimerMax * textToInput.Length);
        text.text = textToInput.Substring(0, lettersCount);
        if (hardLaneSwitchTimerCurrent >= hardLaneSwitchTimerMax)
        {
            text.alpha = 0;
            textbg.alpha = 0;
            ActivateBonus(true);
        }
    }

    void ActivateBonus(bool value)
    {
        if (value)
        {
            bonusActive = value;
            bonusText.SetActive(value);
            NotifyPointsManager(2);
        }
        else
        {
            bonusActive = value;
            bonusText.SetActive(value);
            NotifyPointsManager(1);
        }
    }

    void NotifyPointsManager(int value)
    {
        PointsManager.instance.UpdateLaneMultiplier(value);
    }
}
