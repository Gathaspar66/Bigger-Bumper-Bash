using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CarHandler : MonoBehaviour
{
    public Rigidbody rb;
    public Transform gameModel;
    public float acceleationMultiplier = 3;
    public float brakeMultiplier = 15;
    public float steeringMultiplier = 5;
    Vector2 input=Vector2.zero;
    public float maxForwardVelocity = 30;
    float maxSteerVelocity = 2;
    
    
    void Update()
    {
        gameModel.transform.rotation = Quaternion.Euler(0, rb.velocity.x * 3, 0);
    }
    private void FixedUpdate()
    {
        if (input.y > 0)
        {
            Accelerate();
            DebugWindow.instance.UpdateDebugWindow(DebugWindowEnum.ACCELLERATING, "true");
            DebugWindow.instance.UpdateDebugWindow(DebugWindowEnum.BRAKING, "false");
        }
        else
        {
            rb.drag = 0.2f;
            DebugWindow.instance.UpdateDebugWindow(DebugWindowEnum.ACCELLERATING, "false");
        }

        if (input.y < 0)
        {
            Brake();
            DebugWindow.instance.UpdateDebugWindow(DebugWindowEnum.ACCELLERATING, "false2");
            DebugWindow.instance.UpdateDebugWindow(DebugWindowEnum.BRAKING, "true");
        }

        Steer();
        if(rb.velocity.z<=0)
        {
            rb.velocity = Vector3.zero;
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
            float speedbaseSteerLimit = rb.velocity.z / 5.0f;
            speedbaseSteerLimit = Mathf.Clamp01(speedbaseSteerLimit);



            rb.AddForce(rb.transform.right * steeringMultiplier * input.x * speedbaseSteerLimit);

            float normalizedX = rb.velocity.x / maxSteerVelocity;
            normalizedX = Mathf.Clamp(normalizedX, -1.0f, 1.0f);
            rb.velocity = new Vector3(normalizedX * maxSteerVelocity, 0, rb.velocity.z);

            if (normalizedX > 0)
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
            rb.velocity = Vector3.Lerp(rb.velocity, new Vector3(0, 0, rb.velocity.z), Time.fixedDeltaTime * 3);
            DebugWindow.instance.UpdateDebugWindow(DebugWindowEnum.TURNING, "false");
        }
    }

    public void SetInput(Vector2 inputVector)
    {
        inputVector.Normalize();
        input = inputVector;


    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("wykryto kolizjê z:");
        //EditorApplication.isPaused = true;
        Time.timeScale = 0;
    }
        
        
        





}
