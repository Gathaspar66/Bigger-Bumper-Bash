using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FakeInputHandler : MonoBehaviour
{
    private Vector2 input = Vector2.zero;
    public GameObject car;

    public FakeJoystickSteering joyL;
    public FakeJoystickAccelerationBrake joyR;

    void Start()
    {
        car = PlayerManager.instance.GetPlayerInstance();
    }

    void Update()
    {
        //joyL.GetComponent<FakeJoystickSteering>().GetInput();
        input = joyR.GetComponent<FakeJoystickAccelerationBrake>().GetInput();


        car.GetComponent<PlayerSteering>().SetInput(input);
    }


    private void UpdateInput()
    {
        car.GetComponent<PlayerSteering>().SetInput(input);
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