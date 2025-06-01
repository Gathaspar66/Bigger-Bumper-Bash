using UnityEngine;

public class PointsDisplay : MonoBehaviour
{
    public DigitRowHandler digitRowHandler;


    public void SetDigits(int points)
    {
        digitRowHandler.UpdatePointsDisplay(points);
    }
}