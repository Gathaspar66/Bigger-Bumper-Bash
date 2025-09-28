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
}
