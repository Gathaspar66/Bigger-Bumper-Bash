using System;
using UnityEngine;

public enum SteeringDirection
{
    LEFT,
    RIGHT,
    FORWARD,
    BRAKE,
    ACCELERATE,
    NEUTRAL
}

public class ControlHandler : MonoBehaviour
{
    private GameObject car;
    public GameObject debugArea;
    private Vector2 input = Vector2.zero;

    public static ControlHandler instance;

    PlayerSteering ps;
    PlayerPrefabHandler phd;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        GeneralSetup();
    }

    private void Update()
    {
        SetInput();
    }

    private void GeneralSetup()
    {
        car = PlayerManager.instance.GetPlayerInstance();
        ps = car.GetComponent<PlayerSteering>();
        phd = car.GetComponent<PlayerPrefabHandler>();
    }

    private void SetInput()
    {
        ps.SetInput(input);
        phd.SetInput(input);
    }

    public void SetInput(SteeringDirection dir)
    {
        switch (dir)
        {
            case SteeringDirection.LEFT:
                input.x = -1;
                break;

            case SteeringDirection.RIGHT:
                input.x = 1;
                break;

            case SteeringDirection.FORWARD:
                input.x = 0;
                break;

            case SteeringDirection.BRAKE:
                input.y = -1;
                break;

            case SteeringDirection.ACCELERATE:
                input.y = 1;
                break;

            case SteeringDirection.NEUTRAL:
                input.y = 0;
                break;
        }
    }

    public void ShowDebugAreas(bool show)
    {
        debugArea.SetActive(show);
    }
}