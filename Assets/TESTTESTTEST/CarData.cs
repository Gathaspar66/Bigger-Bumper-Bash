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
    [Tooltip("Whether the car is unlocked in-game – not loaded from JSON, set dynamically during gameplay.")]
    public bool isUnlocked;

    [Tooltip("Whether the car is available for the player to drive – loaded from JSON.")]
    public bool isAvailable;
    public bool spawnForAI;
    [Header("Car Prefab")]
    public GameObject carPrefab;
    [Header("Other")]
    public string textToUnlock;


}