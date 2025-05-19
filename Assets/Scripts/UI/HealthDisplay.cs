using System.Collections.Generic;
using UnityEngine;

public class HealthDisplay : MonoBehaviour
{

    public GameObject healthPrefab;
    public GameObject emptyHealthPrefab;
    public Transform healthOrganizer;
    private readonly List<GameObject> healthIcons = new();

    public void UpdateHealthVisuals(int health, int maxHealth)
    {
        DeleteOldHealth();


        for (int i = 0; i < maxHealth; i++)
        {
            GameObject prefab = i < health ? healthPrefab : emptyHealthPrefab;
            GameObject healthElement = Instantiate(prefab, healthOrganizer);
            healthIcons.Add(healthElement);
        }
    }
    
    public void DeleteOldHealth()
    {
        foreach (GameObject icon in healthIcons)
        {
            Destroy(icon);
        }
    }
}