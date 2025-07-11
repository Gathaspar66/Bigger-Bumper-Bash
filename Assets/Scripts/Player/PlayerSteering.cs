using System;
using System.Collections.Generic;
using UnityEngine;
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
    private readonly CarModelHandler carModelHandler;
    public Transform gameModel;
    private Vector2 input = Vector2.zero;
    private readonly bool wHeld = false;
    public static PlayerSteering instance;

    private float targetTiltX = 0f;
    private readonly float tiltSpeed = 5f;
    private readonly float tiltValue = 5f;

    bool isPlayerDead = false;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        gameModel = PlayerManager.instance.GetCarInstance().transform;
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

    private void Update()
    {
        // gameModel.transform.rotation = Quaternion.Euler(0, rb.velocity.x * 0.5f, rb.velocity.x * 1f);
    }

    private void FixedUpdate()
    {
        float currentTiltX = gameModel.transform.localEulerAngles.x;
        if (currentTiltX > 180)
        {
            currentTiltX -= 360;
        }

        float newTiltX = Mathf.Lerp(currentTiltX, targetTiltX, Time.deltaTime * tiltSpeed);
        float tiltY = rb.velocity.x * 0.5f;
        float tiltZ = rb.velocity.x * 1f;

        gameModel.transform.localRotation = Quaternion.Euler(newTiltX, tiltY, tiltZ);


        if (wHeld)
        {
            input.y = 1;
        }

        if (input.y > 0)
        {
            Accelerate();
        }
        else
        {
            rb.drag = 0.2f;
        }

        if (input.y < 0)
        {
            Brake();
        }

        Steer();
        if (rb.velocity.z < minForwardVelocity && rb.velocity.z > 0)
        {
            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, minForwardVelocity);
        }

        if (input.y == 0)
        {
            targetTiltX = 0f;
        }

        if (rb.velocity.z <= minForwardVelocity || rb.velocity.z >= maxForwardVelocity)
        {
            targetTiltX = 0f;
        }
    }

    public void Accelerate()
    {
        targetTiltX = -tiltValue;


        rb.drag = 0;
        if (rb.velocity.z >= maxForwardVelocity)
        {
            return;
        }

        rb.AddForce(rb.transform.forward * acceleationMultiplier * input.y);
    }

    internal void SetPlayerDead(bool isDead)
    {
        isPlayerDead = isDead;
    }

    public void Brake()
    {
        targetTiltX = tiltValue;


        if (rb.velocity.z <= 0)
        {
            return;
        }

        rb.AddForce(rb.transform.forward * brakeMultiplier * input.y);
        //carModelHandler.SetRearBrakeLight(isBraking);
    }

    public void Steer()
    {
        if (isPlayerDead) return;

        if (Mathf.Abs(input.x) > 0)
        {
            float currentSpeed = rb.velocity.z;


            float dynamicSteeringMultiplier = steeringMultiplierCurve.Evaluate(currentSpeed);

            float targetXVelocity = input.x * dynamicSteeringMultiplier;


            rb.velocity = new Vector3(
                Mathf.Lerp(rb.velocity.x, targetXVelocity, Time.fixedDeltaTime * 5),
                rb.velocity.y,
                rb.velocity.z
            );
        }
        else
        {
            rb.velocity = Vector3.Lerp(rb.velocity, new Vector3(0, rb.velocity.y, rb.velocity.z),
                Time.fixedDeltaTime * 3);
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
            minForwardVelocity + ((maxForwardVelocity - minForwardVelocity) / 2),
            maxForwardVelocity
        };
    }
}