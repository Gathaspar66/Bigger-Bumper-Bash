using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngameMenu : MonoBehaviour
{
    public GameObject ingameMenu;

    bool isMenuOpen = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleIngameMenu()
    {
        isMenuOpen = !isMenuOpen;
        GameManager.instance.PauseGame(isMenuOpen);
        ingameMenu.SetActive(isMenuOpen);
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
