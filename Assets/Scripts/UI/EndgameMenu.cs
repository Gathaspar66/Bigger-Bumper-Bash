using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class EndgameMenu : MonoBehaviour
{
    public Image blackBackground;
    public TMP_Text highScore;
    public GameObject newHighScoreNotification;

    // Start is called before the first frame update
    void Start()
    {
        ScaleBlackBackground();
    }

    private void ScaleBlackBackground()
    {
        blackBackground.rectTransform.sizeDelta = new Vector2(Screen.width, Screen.height);
    }

    // Update is called once per frame
    void Update()
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
        print("update score " + points+ " " + PlayerPrefs.GetInt("highScore"));
        if (points > PlayerPrefs.GetInt("highScore"))
        {
            newHighScoreNotification.SetActive(true);
        }
        highScore.text = points.ToString();
    }
}
