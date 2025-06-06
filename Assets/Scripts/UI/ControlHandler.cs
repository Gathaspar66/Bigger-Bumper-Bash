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

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        car = PlayerManager.instance.GetPlayerInstance();
    }

    private void Update()
    {
        car.GetComponent<PlayerSteering>().SetInput(input);
        car.GetComponent<PlayerHitDetection>().SetInput(input);
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