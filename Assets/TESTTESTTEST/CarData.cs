using UnityEngine;

public enum CarType
{
    OGIER,
    UNIKACZ,
    MOTORCAR
}

[CreateAssetMenu]
public class CarData : ScriptableObject
{
    [Header("Dane Auta")]
    public CarType carType;

    public string nazwa;
    public int hp;

    [Header("Prefab Auta")]
    public GameObject prefabAuta;
}