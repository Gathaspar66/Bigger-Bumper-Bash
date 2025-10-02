using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMaterialManager : MonoBehaviour
{
    public static CarMaterialManager instance;

    public List<CarMaterialData> materials = new();

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public CarMaterialData GetMaterialByInt(int value)
    {
        return GetMaterialByType((CarMaterialType)value);
    }

    public CarMaterialData GetMaterialByType(CarMaterialType value)
    {
        foreach(CarMaterialData i in materials)
        {
            if (i.matType == value)
            {
                return i;
            }
        }
        return null;
    }
}
