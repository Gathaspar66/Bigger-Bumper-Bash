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
    public int maxMultplier = 18;
    private float lastCarLocationZ;
    private float currentCarLocationZ;
    private float pointsMultiplier = 0;

    public float points = 0;
    private bool calculatePoints = true;

    private void Awake()
    {
        instance = this;
    }


    public void Activate()
    {
        car = PlayerManager.instance.GetPlayerInstance();
        lastCarLocationZ = car.transform.position.z;
    }

    private void Update()
    {
        CheckCrateMultiplierTimer();
        CalculatePoints();
    }

    private void CalculatePoints()
    {
        if (!calculatePoints)
        {
            return;
        }

        currentCarLocationZ = car.transform.position.z;
        pointsMultiplier = speedPointMultiplier * lanePointMultiplier * cratePointMultiplier;
        UserInterfaceManager.instance.UpdatePointsMultiplierDisplay((int)pointsMultiplier);
        points += (currentCarLocationZ - lastCarLocationZ) * pointsMultiplier / 10;
        UserInterfaceManager.instance.UpdatePointsDisplay((int)points);
        lastCarLocationZ = car.transform.position.z;
    }

    public void AddPoints(float pointValue)
    {
        points += pointValue;

        CrateHit();
    }

    public void UpdateSpeedMultiplier(int value)
    {
        speedPointMultiplier = value;
    }

    public void UpdateLaneMultiplier(int value)
    {
        lanePointMultiplier = value;
    }

    public void CrateHit()
    {
        cratePointMultiplierTimer = cratePointMultiplierTimerMax;
        if (cratePointMultiplier != cratePointMultiplierMax) //if crate multiplier not max, add 1
        {
            cratePointMultiplier += 1;
            //if crate multiplier above max, set to max
            if (cratePointMultiplier > cratePointMultiplierMax)
            {
                cratePointMultiplier = cratePointMultiplierMax;
            }
        }
    }

    private void CheckCrateMultiplierTimer()
    {
        if (cratePointMultiplier == 1)
        {
            return;
        }

        cratePointMultiplierTimer -= Time.deltaTime;
        if (cratePointMultiplierTimer < 0)
        {
            cratePointMultiplierTimer = cratePointMultiplierTimerMax;
            if (cratePointMultiplier > 1)
            {
                cratePointMultiplier -= 1;
            }
        }
        float progress = Mathf.Clamp01(cratePointMultiplierTimer / cratePointMultiplierTimerMax);
        UserInterfaceManager.instance.UpdatePickupMultiplierFill(progress);
        UserInterfaceManager.instance.UpdatePickupMultiplierVisual((int)cratePointMultiplier);
    }

    public void SavePlayerScore()
    {
        calculatePoints = false;
        if (points <= PlayerPrefs.GetInt("highScore"))
        {
            return;
        }

        PlayerPrefs.SetInt("highScore", (int)points);
    }

    public int GetPoints()
    {
        return (int)points;
    }
}