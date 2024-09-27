using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InputHandler : MonoBehaviour
{
    private Vector2 input = Vector2.zero;
    public CarHandler carHandler;
    public TMP_Dropdown controlDropdown;
    public GameObject joystick; 
    public GameObject buttonControls;
    void Update()
    {

        if (JoyStick.Instance != null && JoyStick.Instance.Background.gameObject.activeSelf)
        {
      
            input = JoyStick.Instance.GetInput();
        }


        carHandler.SetInput(input);

     
    }

    public void SetHorizontal(float value)
    {
        input.x = value;
        UpdateInput();
    }

  
    public void SetVertical(float value)
    {
        input.y = value;
        UpdateInput();
    }

  
    public void ResetCar()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    
    private void UpdateInput()
    {
        carHandler.SetInput(input);
    }
    public void UpdateControlScheme()
    {
       
        switch (controlDropdown.value)
        {
            case 0:
                joystick.SetActive(true);
                buttonControls.SetActive(false);
                break;

            case 1:
                joystick.SetActive(false);
                buttonControls.SetActive(true);
                break;

            default:
                joystick.SetActive(false);
                buttonControls.SetActive(false);
                break;
        }
    }

}
