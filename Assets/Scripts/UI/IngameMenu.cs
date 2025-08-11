using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class IngameMenu : MonoBehaviour
{
    public GameObject ingameMenu;

    public GameObject menuOpenButton;
    public GameObject menuCloseButton;

    bool isMenuOpen = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenIngameMenu()
    {
        GameManager.instance.PauseGame(true);
        ingameMenu.SetActive(true);
        menuOpenButton.SetActive(false);
        menuCloseButton.SetActive(true);
        SoundManager.instance.MuteEngineSound(true);
    }

    public void CloseIngameMenu()
    {
        GameManager.instance.PauseGame(false);
        ingameMenu.SetActive(false);
        menuOpenButton.SetActive(true);
        menuCloseButton.SetActive(false);
        SoundManager.instance.MuteEngineSound(false);

    }

    public void OnRestartButtonPressed()
    {
        GameManager.instance.RestartLevel();
    }

    public void OnQuitButtonPressed()
    {
        GameManager.instance.QuitToMenu();
    }
}
