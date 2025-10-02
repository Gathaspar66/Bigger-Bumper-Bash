using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarMaterialButton : MonoBehaviour
{
    public Image markOutline;
    public GameObject xMarker;
    public Button button;

    public CarMaterialType carMatType;
    public bool unlocked = false;

    public CarMaterialsBar cmb;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetSelected(bool value)
    {
        markOutline.color = value ? Color.yellow : Color.white;
    }

    public void SetMaterialColor(Color color)
    {
        button.gameObject.GetComponent<Image>().color = color;
    }

    public void SetUnlocked(bool value)
    {
        xMarker.SetActive(!value);
        unlocked = !value;
    }

    public void SetMaterialType(CarMaterialType value)
    {
        carMatType = value;
    }

    public void SetCarMaterialsBar(CarMaterialsBar value)
    {
        cmb = value;
    }

    public void OnButtonPress()
    {
        cmb.ButtonPressed(carMatType);
        SetSelected(true);
    }
}
