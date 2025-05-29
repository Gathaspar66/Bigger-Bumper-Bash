using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public DigitRowHandler digitRowHandler;

    public void Start()
    {
        UpdateHighScoreText();
    }

    public void OnGameStartButtonPressed()
    {
        SceneManager.LoadScene("Level1");
    }

    private void UpdateHighScoreText()
    {
        if (PlayerPrefs.GetInt("highScore") == 0)
        {
            return;
        }

        int points = PlayerPrefs.GetInt("highScore");
        digitRowHandler.UpdatePointsDisplay(points);
    }
}