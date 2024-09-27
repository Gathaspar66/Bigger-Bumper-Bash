using System.Collections;
using System.Collections.Generic;
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
    void Start()
    {
        
    }

    
    void Update()
    {
        gameModel.transform.rotation = Quaternion.Euler(0, rb.velocity.x * 3, 0);
    }
    private void FixedUpdate()
    {
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
            speedbaseSteerLimit=Mathf.Clamp01(speedbaseSteerLimit);



            rb.AddForce(rb.transform.right * steeringMultiplier * input.x* speedbaseSteerLimit);

            float normalizedX = rb.velocity.x / maxSteerVelocity;
            normalizedX=Mathf.Clamp(normalizedX,-1.0f,1.0f);
            rb.velocity = new Vector3(normalizedX * maxSteerVelocity, 0, rb.velocity.z);

        }
        else
        {
            rb.velocity = Vector3.Lerp(rb.velocity, new Vector3(0, 0, rb.velocity.z), Time.fixedDeltaTime * 3);
        }
    }
    public void SetInput(Vector2 inputVector)
    {
        inputVector.Normalize();
        input = inputVector;


    }

}
