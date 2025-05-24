using TMPro;
using UnityEngine;

public class PointsDisplay : MonoBehaviour
{
    public TMP_Text text;


    public void UpdatePointsDisplay(float points)
    {
        text.text = "" + (int)points;
    }
}