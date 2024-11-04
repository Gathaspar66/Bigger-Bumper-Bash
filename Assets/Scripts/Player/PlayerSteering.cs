using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using static CarConfiguration;


public class PlayerSteering : MonoBehaviour
{
    [Header("Car Performance Settings")] //
    public float acceleationMultiplier;

    public float brakeMultiplier;
    public float steeringMultiplier;
    public float minForwardVelocity;
    public float maxForwardVelocity;
    public float maxSteerVelocity;

    public AnimationCurve steeringMultiplierCurve;

    [Header("Other")] //
    public Rigidbody rb;

    public Transform gameModel;
    private Vector2 input = Vector2.zero;
    private bool wHeld = false;
    public static PlayerSteering instance;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        gameModel = PlayerManager.instance.GetPlayerInstance().transform;
        rb.velocity = new Vector3(0, 0, minForwardVelocity);
    }


    public void LoadCarSettings(CarConfig config)
    {
        acceleationMultiplier = config.acceleationMultiplier;
        brakeMultiplier = config.brakeMultiplier;
        steeringMultiplier = config.steeringMultiplier;
        maxForwardVelocity = config.maxForwardVelocity;
        maxSteerVelocity = config.maxSteerVelocity;
        minForwardVelocity = config.minForwardVelocity;
    }

    void Update()
    {
        gameModel.transform.rotation = Quaternion.Euler(0, rb.velocity.x * 0.5f, rb.velocity.x * 1f);


        if (Input.GetKeyDown(KeyCode.W))
        {
            wHeld = true;
        }

        if (Input.GetKeyUp(KeyCode.W))
        {
            wHeld = false;
        }
    }

    private void FixedUpdate()
    {
        DebugWindow.instance.UpdateDebugWindow(DebugWindowEnum.ACCELLERATING, "false");
        DebugWindow.instance.UpdateDebugWindow(DebugWindowEnum.BRAKING, "false");
        if (wHeld)
        {
            input.y = 1;
        }

        if (input.y > 0)
        {
            Accelerate();
            DebugWindow.instance.UpdateDebugWindow(DebugWindowEnum.ACCELLERATING, "true");
        }
        else
        {
            rb.drag = 0.2f;
        }

        if (input.y < 0)
        {
            Brake();

            DebugWindow.instance.UpdateDebugWindow(DebugWindowEnum.BRAKING, "true");
        }

        Steer();
        if (rb.velocity.z < minForwardVelocity && rb.velocity.z > 0)
        {
            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, minForwardVelocity);
        }
    }

    public void Accelerate()
    {
        rb.drag = 0;
        if (rb.velocity.z >= maxForwardVelocity)
            return;
        rb.AddForce(rb.transform.forward * acceleationMultiplier * input.y);
    }

    public void Brake()
    {
        if (rb.velocity.z <= 0)
            return;


        rb.AddForce(rb.transform.forward * brakeMultiplier * input.y);
    }

    public void Steer()
    {
        if (Mathf.Abs(input.x) > 0)
        {
            float currentSpeed = rb.velocity.z;


            float dynamicSteeringMultiplier = steeringMultiplierCurve.Evaluate(currentSpeed);
            print(dynamicSteeringMultiplier);

            float targetXVelocity = input.x * dynamicSteeringMultiplier;


            rb.velocity = new Vector3(
                Mathf.Lerp(rb.velocity.x, targetXVelocity, Time.fixedDeltaTime * 5),
                rb.velocity.y,
                rb.velocity.z
            );


            if (input.x > 0)
            {
                DebugWindow.instance.UpdateDebugWindow(DebugWindowEnum.TURNING, "right");
            }
            else
            {
                DebugWindow.instance.UpdateDebugWindow(DebugWindowEnum.TURNING, "left");
            }
        }
        else
        {
            rb.velocity = Vector3.Lerp(rb.velocity, new Vector3(0, rb.velocity.y, rb.velocity.z),
                Time.fixedDeltaTime * 3);


            DebugWindow.instance.UpdateDebugWindow(DebugWindowEnum.TURNING, "false");
        }
    }


    public void SetInput(Vector2 inputVector)
    {
        inputVector.Normalize();
        input = inputVector;
    }

    public List<float> GetSpeedBreakpoints()
    {
        return new List<float>
        {
            minForwardVelocity,
            minForwardVelocity + (maxForwardVelocity - minForwardVelocity) / 2,
            maxForwardVelocity
        };
    }
}