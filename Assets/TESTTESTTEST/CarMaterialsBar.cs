using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CarMaterialsBar : MonoBehaviour
{
    public Image segmentPrefab;
    public Image xSegmentPrefab;
    public TMP_Text textToUnlock;
    public List<Image> segments = new();
    int currentMaterialIndex = 0;

    public GameObject startButton;

    List<CarMaterialData> availableMaterials = new();

    // Start is called before the first frame update
    void Start()
    {
        BuildBar();
    }
    private void OnEnable()
    {
        BuildBar();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void BuildBar()
    {
        foreach (Image i in segments)
        {
            Destroy(i.gameObject);
        }
        segments.Clear();
        currentMaterialIndex = 0; 
        availableMaterials.Clear();

         CarType selectedCar = (CarType)PlayerPrefs.GetInt("SelectedCar");

        for (int i = 0; i < CarMaterialManager.instance.materials.Count; i++)
        {
            if (CarMaterialManager.instance.materials[i].unlockedForCars.Contains(selectedCar))
            {
                availableMaterials.Add(CarMaterialManager.instance.materials[i]);
                Image seg = Instantiate(segmentPrefab, transform);
                seg.name = $"Seg{i + 1}";
                segments.Add(seg);
                Image seg2 = Instantiate(segmentPrefab, seg.transform);
                seg2.color = CarMaterialManager.instance.materials[i].matColor;
                seg2.rectTransform.transform.localScale = new Vector2(0.9f, 0.9f);
                if (!CarMaterialManager.instance.materials[i].isUnlocked)
                {
                    Image seg3 = Instantiate(xSegmentPrefab, seg.transform);
                }
            }
        }
        SetCurrentlyChosenMaterial();
    }

    public void NextMaterial()
    {
        if (currentMaterialIndex >= availableMaterials.Count - 1)
        {
            currentMaterialIndex = 0;
        }
        else
        {
            currentMaterialIndex++;
        }
        SetCurrentlyChosenMaterial();
    }

    internal void PreviousMaterial()
    {
        if (currentMaterialIndex <= 0)
        {
            currentMaterialIndex = availableMaterials.Count - 1;
        }
        else
        {
            currentMaterialIndex--;
        }
        SetCurrentlyChosenMaterial();
    }

    private void SetCurrentlyChosenMaterial()
    {
        //CarMaterialManager.instance.materials[currentMaterialIndex];
        foreach(Image i in segments)
        {
            i.color = Color.white;
        }
        segments[currentMaterialIndex].color = Color.green;
        MainMenuManager.instance.GetCurrentCar().SetCarMaterial(availableMaterials[currentMaterialIndex].matObject);
        startButton.SetActive(availableMaterials[currentMaterialIndex].isUnlocked);
        textToUnlock.text = availableMaterials[currentMaterialIndex].isUnlocked ? "" : availableMaterials[currentMaterialIndex].textToUnlock;

        PlayerPrefs.SetInt("CarColorChoice", (int)availableMaterials[currentMaterialIndex].matType);
        PlayerPrefs.Save();
    }
}
