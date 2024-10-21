using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PickupMultiplier : MonoBehaviour
{
    public List<Sprite> barrelSprites = new();
    public Image barrelImage;
    public TMP_Text text;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UpdatePickupVisual(int value)
    {
        text.text = "x" + value.ToString();
        barrelImage.sprite = barrelSprites[value - 1];
    }

    public void UpdatePickupMultiplier(int value)
    {
        UpdatePickupVisual(value);
    }
}
