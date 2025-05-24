using UnityEngine;
using UnityEngine.UI;

public class WrongWay : MonoBehaviour
{
    private GameObject car;
    private bool hardLane = false;
    private bool bonusActive = false;

    public float hardLaneSwitchTimerCurrent = 0;
    private readonly float hardLaneSwitchTimerMax = 1f;

    public Image coloredImage;
    public Image fillImage;
    public Image multiplierImage;
    public Image multiplierValueImage;

    private void Start()
    {
        car = PlayerManager.instance.GetPlayerInstance();
        HideVisuals();
    }

    private void Update()
    {
        car = PlayerManager.instance.GetPlayerInstance();
        CheckLane();
        UpdateHardLane();
    }

    private void CheckLane()
    {
        if (car.transform.position.x < 0)
        {
            if (!hardLane)
            {
                EnterHardLane();
            }
        }
        else
        {
            if (hardLane)
            {
                ExitHardLane();
            }
        }
    }

    private void EnterHardLane()
    {
        hardLane = true;
        ShowVisuals();
    }

    private void ExitHardLane()
    {
        hardLane = false;
        bonusActive = false;
        hardLaneSwitchTimerCurrent = 0;
        SetFill(0);
        HideVisuals();
        NotifyPointsManager(1);
    }

    private void UpdateHardLane()
    {
        if (!hardLane || bonusActive)
        {
            return;
        }

        hardLaneSwitchTimerCurrent += Time.deltaTime;
        float progress = Mathf.Clamp01(hardLaneSwitchTimerCurrent / hardLaneSwitchTimerMax);
        SetFill(progress);

        if (progress >= 1f)
        {
            bonusActive = true;
            ShowBonus();
            NotifyPointsManager(2);
        }
    }

    private void SetFill(float value)
    {
        fillImage.fillAmount = value;
    }

    private void ShowVisuals()
    {
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

    private void ShowBonus()
    {
        multiplierImage.gameObject.SetActive(true);
        multiplierValueImage.gameObject.SetActive(true);
    }

    private void NotifyPointsManager(int multiplier)
    {
        PointsManager.instance.UpdateLaneMultiplier(multiplier);
    }
}