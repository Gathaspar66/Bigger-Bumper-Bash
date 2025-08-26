using System.Collections.Generic;
using UnityEngine;

public class CarDatabaseManager : MonoBehaviour
{
    public static CarDatabaseManager instance;

    public TextAsset carJsonFile;

    public List<CarData> carObjects = new();
    private List<CarDataModel> cars = new();

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    public void Activate()
    {
        if (!CarDataLoader.IsLoaded())
        {
            LoadCars();
            ApplyDataToScriptableObjects();
        }
        PrintLoadedCars();
    }

    private void LoadCars()
    {
        cars = CarDataLoader.LoadCarsFromJson(carJsonFile);
    }

    public void PrintLoadedCars()
    {
        if (cars == null || cars.Count == 0)
        {
            Debug.Log("Lista samochodów jest pusta!");
            return;
        }

        Debug.Log("Wczytane samochody:");

        foreach (CarDataModel car in cars)
        {
            Debug.Log($"CarType: {car.carType}, Nazwa: {car.name}, HP: {car.hp}");
        }
    }

    private void ApplyDataToScriptableObjects()
    {
        foreach (CarData carSO in carObjects)
        {
            foreach (CarDataModel car in cars)
            {
                if (carSO.carType == car.carType)
                {
                    carSO.name = car.name;
                    carSO.hp = car.hp;

                    carSO.acceleationMultiplier = car.acceleationMultiplier;
                    carSO.brakeMultiplier = car.brakeMultiplier;
                    carSO.steeringMultiplier = car.steeringMultiplier;
                    carSO.maxForwardVelocity = car.maxForwardVelocity;
                    carSO.maxSteerVelocity = car.maxSteerVelocity;
                    carSO.minForwardVelocity = car.minForwardVelocity;

                    carSO.isUnlocked = car.isUnlocked;
                    carSO.isAvailable = car.isAvailable;

                    break;
                }
            }
        }
    }
}