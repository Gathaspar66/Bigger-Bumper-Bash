using System.Collections.Generic;
using UnityEngine;

public enum CarMaterialType
{
    RED,
    BLUE,
    YELLOW,
    PINK,
    WHITE,
    BROWN,
}

[CreateAssetMenu]
public class CarMaterialData : ScriptableObject
{
    [Header("Basic Material Info")]
    public CarMaterialType matType;

    public Color matColor;

    public new string name;

    [Header("Material Availability")]
    [Tooltip("Whether the material is unlocked in-game – not loaded from JSON, set dynamically during gameplay.")]
    public bool isUnlocked;

    public List<CarType> unlockedForCars;

    //[Tooltip("Whether the material is available for the AI to use – loaded from JSON.")]
    //public bool isAvailable;

    //public bool spawnForAI;

    [Header("Material Object")]
    public Material matObject;

    [Header("Other")]
    public string textToUnlock;
}