using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class turnoffoutline : MonoBehaviour
{
    public List<GameObject> outlines;
    public List<GameObject> cars;
    public List<GameObject> carBodies;
    public List<Material> normalMaterials;
    public List<Material> toonMaterials;

    public float rotationSpeed = 10;

    public bool outlineVisible = true;
    public bool rotating = false;
    public bool usingNormalMaterials = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!rotating) return;
        foreach(GameObject i in cars)
        {
            i.transform.Rotate(new Vector3(0, 1, 0) * Time.deltaTime * rotationSpeed);
        }
    }

    public void ToggleOutlines()
    {
        outlineVisible = !outlineVisible;
        foreach (GameObject i in outlines)
        {
            i.SetActive(outlineVisible);
        }
    }

    public void ToggleRotation()
    {
        rotating = !rotating;
    }
    public void ToggleMaterials()
    {
        usingNormalMaterials = !usingNormalMaterials;
        for (int i = 0; i < carBodies.Count; i++)
        {
            if (usingNormalMaterials)
            {
                carBodies[i].GetComponent<MeshRenderer>().material = normalMaterials[i];
            }
            else
            {
                carBodies[i].GetComponent<MeshRenderer>().material = toonMaterials[i];
            }
        }
    }
}
