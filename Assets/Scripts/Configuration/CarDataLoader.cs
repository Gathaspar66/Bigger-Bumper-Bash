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
    public float minForwardVelocity;

    public bool isUnlocked;
    public bool isAvailable;
    public bool spawnForAI;
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
    public float minForwardVelocity;

    public bool isUnlocked;
    public bool isAvailableForPlayer;
    public bool spawnForAI;
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
                    //maxSteerVelocity = jsonCar.maxSteerVelocity,
                    minForwardVelocity = jsonCar.minForwardVelocity,
                    //isUnlocked = jsonCar.isUnlocked,
                    isAvailable = jsonCar.isAvailableForPlayer,
                    spawnForAI = jsonCar.spawnForAI,
                    carType = (CarType)System.Enum.Parse(typeof(CarType), jsonCar.carType.ToUpper())
                };

                carList.Add(car);
            }
        }

        isLoaded = true;
        return carList;
    }

    public static CarMaterialType GetLastSelectedMaterialByCar(CarType carType)
    {
        string[] a = { "UNIKACZ", "OGIER", "MOTORCAR", "PUDZIAN", "PICKUP" };
        return (CarMaterialType)PlayerPrefs.GetInt("LastSelectedMaterial" + a[(int)carType]);
    }

    public static void SaveLastSelectedMaterialByCar(CarType carType, CarMaterialType carMat)
    {
        string[] a = { "UNIKACZ", "OGIER", "MOTORCAR", "PUDZIAN", "PICKUP" };
        PlayerPrefs.SetInt("LastSelectedMaterial" + a[(int)carType], (int)carMat);
    }
}