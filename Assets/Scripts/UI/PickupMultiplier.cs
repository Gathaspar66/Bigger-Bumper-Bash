using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickupMultiplier : MonoBehaviour
{
    public List<Sprite> barrelSprites = new();
    public List<Sprite> barrelSpritesGray = new();
    public List<Sprite> barrelMultiplier = new();
    public Image barrelImage;
    public Image barrelImageGray;
    public Image multiplierValueImage;

    public void UpdatePickupMultiplierVisual(int value)
    {
        multiplierValueImage.sprite = barrelMultiplier[value - 1];
        barrelImage.sprite = barrelSprites[value - 1];
        barrelImageGray.sprite = barrelSpritesGray[value - 1];
    }

    public void UpdatePickupMultiplierFill(float value)
    {
        barrelImage.fillAmount = value;
    }
}