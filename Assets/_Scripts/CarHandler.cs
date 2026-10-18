using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEditor;
using System.Reflection.Emit;
using System;

public class CarHandler : MonoBehaviour
{
    [Header("Speed Settings")]
    [SerializeField] float accelerationSpeed = 3;
    [SerializeField] float brakePower = 15;
    [SerializeField] float steerPower = 5;
    private Rigidbody rb;
    private Vector2 input = Vector2.zero;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void SetInput(Vector2 inputVector)
    {
        inputVector.Normalize();

        input = inputVector;
    }

    private void FixedUpdate()
    {
        if(input.y > 0)
        {
            Accelerate();
        }
        else
        {
            rb.linearDamping = .2f;
        }
        // apply brake
        if(input.y < 0) Brake();

        Steer();    

    }

    private void Accelerate()
    {
        rb.linearDamping = 0;
        rb.AddForce(rb.transform.forward * input.y * accelerationSpeed);
    }

    private void Brake()
    {
        // dont brake if we're moving forward
        if(rb.linearVelocity.z <= 0) return;

        rb.AddForce(rb.transform.forward * input.y * brakePower);
    }

    private void Steer()
    {
        if(Mathf.Abs(input.x) > 0)
        {
            rb.AddForce(rb.transform.right * input.x * steerPower);
        }
    }


}
