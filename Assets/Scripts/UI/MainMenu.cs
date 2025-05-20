using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu: MonoBehaviour
{
    public TMP_Text highScoreText;

    public void Start()
    {
        UpdateHighScoreText();
    }

    public void OnGameStartButtonPressed()
    {
        SceneManager.LoadScene("Level1");
    }

    void UpdateHighScoreText()
    {
        if (PlayerPrefs.GetInt("highScore") == 0) return;
        highScoreText.text = PlayerPrefs.GetInt("highScore").ToString();
    }
}