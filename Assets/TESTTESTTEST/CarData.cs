using UnityEngine;

public enum CarType
{
    UNIKACZ,
    OGIER,
    MOTORCAR,
    PUDZIAN,
    PICKUP
}

[CreateAssetMenu]
public class CarData : ScriptableObject
{
    [Header("Basic Car Info")]
    public CarType carType;
    public new string name;
    public int hp;

    [Header("Car Physics")]
    public float acceleationMultiplier;

    public float brakeMultiplier;
    public float steeringMultiplier;
    public float maxForwardVelocity;
    public float maxSteerVelocity;
    public float minForwardVelocity;

    [Header("Car Availability")]
    public bool isUnlocked;

    public bool isAvailable;

    [Header("Car Prefab")]
    public GameObject prefabAuta;
}