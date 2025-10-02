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
    CarMaterialType currentSelectedMaterialType;
    CarType currentSelectedCar;

    public GameObject startButton;


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

        currentSelectedCar = (CarType)PlayerPrefs.GetInt("SelectedCar");
        currentSelectedMaterialType = CarDataLoader.GetLastSelectedMaterialByCar(currentSelectedCar);

        for (int i = 0; i < CarMaterialManager.instance.materials.Count; i++)
        {
            CarMaterialButton currentCMB;
            if (CarMaterialManager.instance.materials[i].unlockedForCars.Contains(currentSelectedCar))
            {
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
        CarMaterialData currentMaterialData = CarMaterialManager.instance.GetMaterialByType(currentSelectedMaterialType);
        MainMenuManager.instance.GetCurrentCar().SetCarMaterial(currentMaterialData.matObject);
        MainMenuManager.instance.StartGameButtonUnlock(currentMaterialData.isUnlocked);
        textToUnlock.text = currentMaterialData.isUnlocked ? "" : currentMaterialData.textToUnlock;
        MarkButtonSelectedByMaterialType();

        PlayerPrefs.SetInt("CarColorChoice", (int)currentMaterialData.matType);
        CarDataLoader.SaveLastSelectedMaterialByCar(currentSelectedCar, currentSelectedMaterialType);
        PlayerPrefs.Save();
    }

    public void ButtonPressed(CarMaterialType value)
    {
        currentSelectedMaterialType = value;
        SetCurrentlyChosenMaterial();
    }

    void MarkButtonSelectedByMaterialType()
    {
        CarMaterialButton currentCMB;
        foreach (GameObject i in segments)
        {
            currentCMB = i.GetComponent<CarMaterialButton>();
            currentCMB.SetSelected(currentCMB.carMatType == currentSelectedMaterialType);
        }
    }
}
