using UnityEngine;

public class PointsMultiplierDisplay : MonoBehaviour
{

    public DigitRowHandler digitRowHandler;

    public void UpdatePointsMultiplierDisplay(int multiplier)
    {
        digitRowHandler.UpdatePointsDisplay(multiplier);
    }
}