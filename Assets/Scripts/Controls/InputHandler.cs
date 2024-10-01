using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InputHandler : MonoBehaviour
{
    private Vector2 input = Vector2.zero;
    public GameObject car;

    void Start()
    {
        car = ControlsCameraChoice.instance.GetCar();
    }

    void Update()
    {
        if (JoyStick.Instance != null && JoyStick.Instance.Background.gameObject.activeSelf)
        {
            input = JoyStick.Instance.GetInput();
        }

        if (JoyStickAutomaticAcceleration.Instance != null)
        {
            input = JoyStickAutomaticAcceleration.Instance.GetInput();
        }

        car.GetComponent<CarHandler>().SetInput(input);
    }


    private void UpdateInput()
    {
        car.GetComponent<CarHandler>().SetInput(input);
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
}