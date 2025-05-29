using UnityEngine;
using UnityEngine.UI;

public class EndgameMenu : MonoBehaviour
{
    public Image blackBackground;
    public GameObject newHighScoreNotification;
    public DigitRowHandler digitRowHandler;
    private void Start()
    {
        ScaleBlackBackground();
    }

    private void ScaleBlackBackground()
    {
        blackBackground.rectTransform.sizeDelta = new Vector2(Screen.width, Screen.height);
    }

    private void Update()
    {
    }

    public void OnRestartButtonPressed()
    {
        GameManager.instance.RestartLevel();
    }

    public void OnQuitButtonPressed()
    {
        GameManager.instance.QuitToMenu();
    }

    public void UpdateScore()
    {
        int points = PointsManager.instance.GetPoints();
        if (points > PlayerPrefs.GetInt("highScore"))
        {
            newHighScoreNotification.SetActive(true);
        }

        UpdatePointsDisplay(points);
    }

    public void UpdatePointsDisplay(int points)
    {
        digitRowHandler.UpdatePointsDisplay(points);
    }


}