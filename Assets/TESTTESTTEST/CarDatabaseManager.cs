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
            Debug.Log($"CarType: {car.carType}, Nazwa: {car.nazwa}, HP: {car.hp}");
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
                    carSO.nazwa = car.nazwa;
                    carSO.hp = car.hp;
                    break; // ju¿ znaleziono pasuj¹cy model
                }
            }
        }
    }
}