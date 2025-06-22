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

    float shakeCooldown = 0.5f;
    float lastShake = 0;
    float shakeSpeed = 1;
    bool shakeLeft = false;

    private void Update()
    {
        ShakeBarrel();
    }

    void ShakeBarrel()
    {
        if (shakeSpeed == 1) return;

        if (lastShake + shakeCooldown < Time.time)
        {
            lastShake = Time.time;
            if (shakeLeft)
            {
                barrelImageGray.rectTransform.rotation = Quaternion.Euler(new Vector3(0, 0, -30));
            }
            else
            {
                barrelImageGray.rectTransform.rotation = Quaternion.Euler(new Vector3(0, 0, 30));
            }
            shakeLeft = !shakeLeft;
        }
    }

    public void UpdatePickupMultiplierVisual(int value)
    {
        multiplierValueImage.sprite = barrelMultiplier[value - 1];
        barrelImage.sprite = barrelSprites[value - 1];
        barrelImageGray.sprite = barrelSpritesGray[value - 1];
        shakeSpeed = value;
        //base cd + scaling cd, the bigger scaling, the bigger increase with level
        shakeCooldown = 0.15f + 0.35f / value;
    }

    public void UpdatePickupMultiplierFill(float value)
    {
        barrelImage.fillAmount = value;
    }
}