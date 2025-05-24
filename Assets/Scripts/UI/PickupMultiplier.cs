using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickupMultiplier : MonoBehaviour
{
    public List<Sprite> barrelSprites = new();
    public List<Sprite> barrelMultiplier = new();
    public Image barrelImage;
    public Image multiplierValueImage;

    private void UpdatePickupVisual(int value)
    {
        multiplierValueImage.sprite = barrelMultiplier[value - 1];
        barrelImage.sprite = barrelSprites[value - 1];
    }

    public void UpdatePickupMultiplier(int value)
    {
        UpdatePickupVisual(value);
    }
}