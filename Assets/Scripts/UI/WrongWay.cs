using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WrongWay : MonoBehaviour
{
    private GameObject car;
    public bool hardLane = false;
    public bool bonusActive = false;

    public float hardLaneSwitchTimerCurrent = 0;
    private readonly float hardLaneSwitchTimerMax = 1f;

    public List<Sprite> bonusNumbersList = new ();

    public Image coloredImage;
    public Image fillImage;
    public Image multiplierImage;
    public Image multiplierValueImage;

    public float hardLaneProgress = 0;

    float shakeCooldown = 0.5f;
    float lastShake = 0;
    bool shakeLeft = false;

    public Vector3 loc;

    private void Start()
    {
        GeneralSetup();
        HideVisuals();
    }

    private void Update()
    {
        loc = transform.position;
        CheckLane();
        UpdateHardLane();
        ShakeLrongWane();
    }

    void ShakeLrongWane()
    {
        if (!hardLane) return;

        if (lastShake + shakeCooldown < Time.time)
        {
            lastShake = Time.time;
            if (shakeLeft)
            {
                coloredImage.rectTransform.rotation = Quaternion.Euler(new Vector3(0, 0, -15));
            }
            else
            {
                coloredImage.rectTransform.rotation = Quaternion.Euler(new Vector3(0, 0, 15));
            }
            shakeLeft = !shakeLeft;
        }
    }

    private void GeneralSetup()
    {
        car = PlayerManager.instance.GetPlayerInstance();
    }

    private void CheckLane()
    {
        if (car.transform.position.x < 0)
        {
            SetHardLine(true);
        }
        else
        {
            SetHardLine(false);
        }
    }

    private void SetHardLine(bool ifInHardLane)
    {
        hardLane = ifInHardLane;
    }

    private void UpdateHardLane()
    {
        if (!hardLane && hardLaneProgress <= 0) return;

        if (hardLane)
        {
            hardLaneSwitchTimerCurrent += Time.deltaTime;
        }
        else
        {
            hardLaneSwitchTimerCurrent -= Time.deltaTime;
        }
        hardLaneSwitchTimerCurrent = Mathf.Clamp01(hardLaneSwitchTimerCurrent);
        hardLaneProgress = Mathf.Clamp01(hardLaneSwitchTimerCurrent / hardLaneSwitchTimerMax);
        SetFill(hardLaneProgress);
        ShowVisuals();

        if (hardLaneProgress >= 1)
        {
            bonusActive = true;
            SetBonus(2);
            NotifyPointsManager(2);
        }
        else
        {
            coloredImage.rectTransform.rotation = Quaternion.identity;
            bonusActive = false;
            SetBonus(1);
            NotifyPointsManager(1);
            if (hardLaneProgress <= 0)
            {
                HideVisuals();
            }
        }
    }

    private void SetFill(float value)
    {
        fillImage.fillAmount = value;
    }

    private void ShowVisuals()
    {
        multiplierImage.gameObject.SetActive(true);
        multiplierValueImage.gameObject.SetActive(true);
        coloredImage.gameObject.SetActive(true);
        fillImage.gameObject.SetActive(true);
    }

    private void HideVisuals()
    {
        coloredImage.gameObject.SetActive(false);
        fillImage.gameObject.SetActive(false);
        multiplierImage.gameObject.SetActive(false);
        multiplierValueImage.gameObject.SetActive(false);
    }

    private void SetBonus(int value)
    {
        multiplierValueImage.sprite = bonusNumbersList[value];
    }

    private void NotifyPointsManager(int multiplier)
    {
        PointsManager.instance.UpdateLaneMultiplier(multiplier);
    }
}