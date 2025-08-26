using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CarDataModel
{
    public CarType carType;
    public string name;
    public int hp;

    public float acceleationMultiplier;
    public float brakeMultiplier;
    public float steeringMultiplier;
    public float maxForwardVelocity;
    public float maxSteerVelocity;
    public float minForwardVelocity;

    public bool isUnlocked;
    public bool isAvailable;
}

[System.Serializable]
public class CarDataWrapperJson
{
    public CarDataWrapperJsonItem[] cars;
}

[System.Serializable]
public class CarDataWrapperJsonItem
{
    public string carType;
    public string name;
    public int hp;

    public float acceleationMultiplier;
    public float brakeMultiplier;
    public float steeringMultiplier;
    public float maxForwardVelocity;
    public float maxSteerVelocity;
    public float minForwardVelocity;

    public bool isUnlocked;
    public bool isAvailable;
}

public static class CarDataLoader
{
    private static bool isLoaded = false;

    public static bool IsLoaded()
    {
        return isLoaded;
    }

    public static List<CarDataModel> LoadCarsFromJson(TextAsset jsonFile)
    {
        List<CarDataModel> carList = new();

        CarDataWrapperJson wrapper = JsonUtility.FromJson<CarDataWrapperJson>(jsonFile.text);

        if (wrapper != null && wrapper.cars != null)
        {
            foreach (CarDataWrapperJsonItem jsonCar in wrapper.cars)
            {
                CarDataModel car = new()
                {
                    name = jsonCar.name,
                    hp = jsonCar.hp,
                    acceleationMultiplier = jsonCar.acceleationMultiplier,
                    brakeMultiplier = jsonCar.brakeMultiplier,
                    steeringMultiplier = jsonCar.steeringMultiplier,
                    maxForwardVelocity = jsonCar.maxForwardVelocity,
                    maxSteerVelocity = jsonCar.maxSteerVelocity,
                    minForwardVelocity = jsonCar.minForwardVelocity,
                    isUnlocked = jsonCar.isUnlocked,
                    isAvailable = jsonCar.isAvailable,
                    carType = (CarType)System.Enum.Parse(typeof(CarType), jsonCar.carType.ToUpper())
                };

                carList.Add(car);
            }
        }

        isLoaded = true;
        return carList;
    }
}