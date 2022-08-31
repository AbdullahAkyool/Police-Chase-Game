using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelController : MonoBehaviour
{
    [SerializeField] private WheelCollider frontRight;
    [SerializeField] private WheelCollider frontLeft;
    [SerializeField] private WheelCollider backRight;
    [SerializeField] private WheelCollider backLeft;

    [SerializeField] private Transform frontRightTransform;
    [SerializeField] private Transform frontLeftTransform;
    [SerializeField] private Transform backRightTransform;
    [SerializeField] private Transform backLeftTransform;

    public float acceleration = 500f;
    public float breakingForce = 300f;
    public float maxTurnAngle = 15f;

    private float currentAcceleration = 0f;
    private float currentBreakingForce = 0f;
    private float currentTurnAngle = 0f;

    public Joystick _Joystick;

    private void FixedUpdate()
    {
        currentAcceleration = acceleration * _Joystick.Vertical*100;
        
        if (Input.GetKey(KeyCode.Space))
        {
            currentBreakingForce = breakingForce;
        }
        else
        {
            currentBreakingForce = 0f;
        }

        backRight.motorTorque = currentAcceleration;
        backLeft.motorTorque = currentAcceleration;
        
        frontRight.brakeTorque = currentBreakingForce;
        frontLeft.brakeTorque = currentBreakingForce;
        backRight.brakeTorque = currentBreakingForce;
        backLeft.brakeTorque = currentBreakingForce;

        currentTurnAngle = maxTurnAngle * _Joystick.Horizontal;
        frontLeft.steerAngle = currentTurnAngle;
        frontRight.steerAngle = currentTurnAngle;

        UpdateWheel(frontRight,frontRightTransform);
        UpdateWheel(frontLeft,frontLeftTransform);
        UpdateWheel(backRight,backRightTransform);
        UpdateWheel(backLeft,backLeftTransform);
    }

    void UpdateWheel(WheelCollider wheelcol, Transform trans)
    {
        Vector3 position;
        Quaternion rotation;
        wheelcol.GetWorldPose(out position, out rotation);
        trans.position = position;
        trans.rotation = rotation;
    }
}
