using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CarDataModel
{
    public CarType carType;
    public string nazwa;
    public int hp;
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
    public string nazwa;
    public int hp;
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

        if (jsonFile == null)
        {
            Debug.LogWarning("Nie podano pliku JSON!");
            return carList;
        }

        CarDataWrapperJson wrapper = JsonUtility.FromJson<CarDataWrapperJson>(jsonFile.text);

        if (wrapper != null && wrapper.cars != null)
        {
            foreach (CarDataWrapperJsonItem jsonCar in wrapper.cars)
            {
                CarDataModel car = new()
                {
                    nazwa = jsonCar.nazwa,
                    hp = jsonCar.hp
                };

                try
                {
                    car.carType = (CarType)System.Enum.Parse(typeof(CarType), jsonCar.carType.ToUpper());
                }
                catch
                {
                    Debug.LogWarning($"Nie uda³o siê sparsowaæ enumu dla {jsonCar.nazwa}, ustawiam OGIER domyœlnie");
                    car.carType = CarType.OGIER;
                }

                carList.Add(car);
                Debug.Log($"Loaded car: {car.carType} - {car.nazwa} - {car.hp}");
            }
        }
        else
        {
            Debug.LogWarning("Nie uda³o siê sparsowaæ JSON lub brak samochodów w pliku!");
        }

        isLoaded = true;
        return carList;
    }
}