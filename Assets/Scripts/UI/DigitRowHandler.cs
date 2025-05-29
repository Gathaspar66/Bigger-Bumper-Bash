using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DigitRowHandler : MonoBehaviour
{
    public Sprite[] digitSprites;
    public List<Image> digitImages;

    public void UpdatePointsDisplay(int points)
    {
        string numStr = points.ToString();
        int numDigits = numStr.Length;

        for (int i = 0; i < digitImages.Count; i++)
        {
            if (i < numDigits)
            {
                int digit = numStr[i] - '0';
                Image img = digitImages[i];

                if (img.sprite != digitSprites[digit])
                {
                    img.sprite = digitSprites[digit];
                }

                SetAlpha(img, 1f);
            }
            else
            {
                SetAlpha(digitImages[i], 0f);
            }
        }
    }

    private void SetAlpha(Image img, float alpha)
    {
        Color c = img.color;
        c.a = alpha;
        img.color = c;
    }
}