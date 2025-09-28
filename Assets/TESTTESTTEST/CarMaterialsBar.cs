using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarMaterialsBar : MonoBehaviour
{
    public Image segmentPrefab;
    public List<Image> segments = new();
    int currentMaterialIndex = 0;

    // Start is called before the first frame update
    void Start()
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
            //Destroy(i.transform.GetChild(0));
            Destroy(i.gameObject);
        }
        segments.Clear();

        for (int i = 0; i < CarMaterialManager.instance.materials.Count; i++)
        {
            Image seg = Instantiate(segmentPrefab, transform);
            seg.name = $"Seg{i + 1}";
            segments.Add(seg);
            Image seg2 = Instantiate(segmentPrefab, seg.transform);
            seg2.color = CarMaterialManager.instance.materials[i].matColor;
            seg2.rectTransform.transform.localScale = new Vector2(0.9f, 0.9f);
        }
        SetCurrentlyChosenMaterial();
    }

    public void NextMaterial()
    {
        if (currentMaterialIndex >= CarMaterialManager.instance.materials.Count - 1)
        {
            currentMaterialIndex = 0;
        }
        else
        {
            currentMaterialIndex++;
        }
        BuildBar();
    }

    internal void PreviousMaterial()
    {
        if (currentMaterialIndex <= 0)
        {
            currentMaterialIndex = CarMaterialManager.instance.materials.Count - 1;
        }
        else
        {
            currentMaterialIndex--;
        }
        BuildBar();
    }

    private void SetCurrentlyChosenMaterial()
    {
        //CarMaterialManager.instance.materials[currentMaterialIndex];
        foreach(Image i in segments)
        {
            segments[currentMaterialIndex].color = Color.white;
        }
        segments[currentMaterialIndex].color = Color.green;
        MainMenuManager.instance.GetCurrentCar().SetCarMaterial(CarMaterialManager.instance.materials[currentMaterialIndex].matObject);
    }
}
