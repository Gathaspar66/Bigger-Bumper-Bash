using UnityEngine;
using UnityEngine.UI;

public class EndgameMenu : MonoBehaviour
{
    public Image blackBackground;
    //public GameObject newHighScoreNotification;
    public DigitRowHandler digitRowHandler;


    public Image scoreImage;
    // public Sprite normalScoreSprite;
    public Sprite highScoreSprite;

    private void Start()
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
            scoreImage.sprite = highScoreSprite;
            scoreImage.color = new Color(255f, 163f, 0f);
        }

        UpdatePointsDisplay(points);
    }

    public void UpdatePointsDisplay(int points)
    {
        digitRowHandler.UpdatePointsDisplay(points);
    }
}