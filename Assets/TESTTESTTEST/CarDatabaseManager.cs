using System.Collections.Generic;
using UnityEngine;

public class CarDatabaseManager : MonoBehaviour
{
    public static CarDatabaseManager instance;

    public TextAsset carJsonFile;

    public List<CarData> carObjects = new();
    private List<CarDataModel> cars = new();
    public List<CarData> aiCarPool = new();

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        PrepareAICarPool();
    }

    public void Activate()
    {
        if (!CarDataLoader.IsLoaded())
        {
            LoadCars();
            ApplyDataToScriptableObjects();
        }
        CheckUnlockConditions();
    }

    private void LoadCars()
    {
        cars = CarDataLoader.LoadCarsFromJson(carJsonFile);
    }

    private void ApplyDataToScriptableObjects()
    {
        Dictionary<CarType, CarDataModel> carDict = new();
        foreach (CarDataModel car in cars)
        {
            if (!carDict.ContainsKey(car.carType))
            {
                carDict.Add(car.carType, car);
            }
        }

        foreach (CarData carSO in carObjects)
        {
            if (carDict.TryGetValue(carSO.carType, out CarDataModel car))
            {
                carSO.name = car.name;
                carSO.hp = car.hp;
                carSO.acceleationMultiplier = car.acceleationMultiplier;
                carSO.brakeMultiplier = car.brakeMultiplier;
                carSO.steeringMultiplier = car.steeringMultiplier;
                carSO.maxForwardVelocity = car.maxForwardVelocity;
                carSO.maxSteerVelocity = car.maxSteerVelocity;
                carSO.minForwardVelocity = car.minForwardVelocity;
                carSO.isAvailable = car.isAvailable;
                carSO.spawnForAI = car.spawnForAI;
                Debug.Log($"Mapped {carSO.carType} -> {carSO.name}, prefab = {carSO.carPrefab.name}");
            }
        }
    }

    private void PrepareAICarPool()
    {
        aiCarPool.Clear();
        foreach (CarData car in carObjects)
        {
            if (car.spawnForAI)
            {
                aiCarPool.Add(car);
            }
        }
    }

    public List<CarData> GetAICarPool()
    {
        return aiCarPool;
    }

    private void CheckUnlockConditions()
    {
        int highScore = PlayerPrefs.GetInt("highScore", 0);
        int collectedBarrels = PlayerPrefs.GetInt("CollectedBarrels", 0);
        int hitedBarriers = PlayerPrefs.GetInt("HitBarriers", 0);
        for (int i = 0; i < carObjects.Count; i++)
        {
            CarData car = carObjects[i];

            switch (car.carType)
            {
                case CarType.UNIKACZ:
                    car.isUnlocked = true;
                    break;

                case CarType.OGIER:
                    car.isUnlocked = collectedBarrels >= 100;
                    break;

                case CarType.MOTORCAR:
                    car.isUnlocked = hitedBarriers >= 17;
                    break;

                case CarType.PUDZIAN:
                    car.isUnlocked = collectedBarrels >= 10 && hitedBarriers >= 17 && highScore >= 10000;
                    break;

                case CarType.PICKUP:
                    car.isUnlocked = highScore >= 100000;
                    break;
            }
        }
    }
}