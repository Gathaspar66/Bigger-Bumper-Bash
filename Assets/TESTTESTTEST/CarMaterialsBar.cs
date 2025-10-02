using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CarMaterialsBar : MonoBehaviour
{
    public GameObject segmentPrefab;
    public TMP_Text textToUnlock;
    public List<GameObject> segments = new();
    int currentMaterialIndex = 0;

    public GameObject startButton;

    List<CarMaterialData> availableMaterials = new();

    // Start is called before the first frame update
    void Start()
    {
        //BuildBar();
    }

    private void OnEnable()
    {
        //BuildBar();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void BuildBar()
    {
        foreach (GameObject i in segments)
        {
            Destroy(i);
        }
        segments.Clear();
        currentMaterialIndex = 0; 
        availableMaterials.Clear();

         CarType selectedCar = (CarType)PlayerPrefs.GetInt("SelectedCar");

        for (int i = 0; i < CarMaterialManager.instance.materials.Count; i++)
        {
            CarMaterialButton currentCMB;
            if (CarMaterialManager.instance.materials[i].unlockedForCars.Contains(selectedCar))
            {
                availableMaterials.Add(CarMaterialManager.instance.materials[i]);
                GameObject seg = Instantiate(segmentPrefab, transform);
                seg.name = $"Seg{i + 1}";
                segments.Add(seg);
                currentCMB = seg.GetComponent<CarMaterialButton>();
                currentCMB.SetUnlocked(CarMaterialManager.instance.materials[i].isUnlocked);
                currentCMB.SetMaterialColor(CarMaterialManager.instance.materials[i].matColor);
                currentCMB.SetMaterialType(CarMaterialManager.instance.materials[i].matType);
                currentCMB.SetCarMaterialsBar(this);
            }
        }
        SetCurrentlyChosenMaterial();
    }


    private void SetCurrentlyChosenMaterial()
    {
        //CarMaterialManager.instance.materials[currentMaterialIndex];
        MainMenuManager.instance.GetCurrentCar().SetCarMaterial(availableMaterials[currentMaterialIndex].matObject);
        startButton.SetActive(availableMaterials[currentMaterialIndex].isUnlocked);
        textToUnlock.text = availableMaterials[currentMaterialIndex].isUnlocked ? "" : availableMaterials[currentMaterialIndex].textToUnlock;

        PlayerPrefs.SetInt("CarColorChoice", (int)availableMaterials[currentMaterialIndex].matType);
        PlayerPrefs.Save();
    }

    public void ButtonPressed(CarMaterialType value)
    {
        currentMaterialIndex = (int)value;
        SetCurrentlyChosenMaterial();
        foreach (GameObject i in segments)
        {
            i.GetComponent<CarMaterialButton>().SetSelected(false);
        }
    }
}
