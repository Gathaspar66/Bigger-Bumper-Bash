using UnityEngine;
using UnityEngine.UI;

public class PointsMultiplierDisplay : MonoBehaviour
{
    public DigitRowHandler digitRowHandler;
    public Image MaxMultiplierImage;

    public void UpdatePointsMultiplierDisplay(int multiplier)
    {
        digitRowHandler.UpdatePointsDisplay(multiplier);
        if (multiplier == PointsManager.instance.maxMultplier)
        {
            MaxMultiplierImage.gameObject.SetActive(true);
        }
        else
        {
            MaxMultiplierImage.gameObject.SetActive(false);
        }
    }
}