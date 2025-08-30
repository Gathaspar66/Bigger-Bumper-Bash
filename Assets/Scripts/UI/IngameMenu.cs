using UnityEngine;

public class IngameMenu : MonoBehaviour
{
    public GameObject ingameMenu;

    public GameObject menuOpenButton;
    public GameObject menuCloseButton;

    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
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