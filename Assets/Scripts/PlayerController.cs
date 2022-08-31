using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private WheelCollider frontRight;
    [SerializeField] private WheelCollider frontLeft;
    [SerializeField] private WheelCollider backRight;
    [SerializeField] private WheelCollider backLeft;

    [SerializeField] private Transform frontRightTransform;
    [SerializeField] private Transform frontLeftTransform;
    [SerializeField] private Transform backRightTransform;
    [SerializeField] private Transform backLeftTransform;
    
    public float speed;
    public float maxSpeed;
    private Vector3 moveForce;
    private float Drag = .98f;
    [SerializeField] private float steerInput;
    [SerializeField] private float steerAngle;
    [SerializeField] private float traction;

    public Joystick _Joystick;

    public float maxTurnAngle = 15f;
    private float currentTurnAngle = 0f;

    public TrailRenderer[] skidMarks;

    public int playerHealth;
    public bool isAlive = true;
    [SerializeField] private ParticleSystem _particleSystemSmoke;
    [SerializeField] private ParticleSystem _particleSystemFire;
    [SerializeField] private ParticleSystem _particleSystemExplosion;
    [SerializeField] private AudioSource burningSound;
    [SerializeField] private AudioSource explosionSound;
    
    void Start()
    {
        Application.targetFrameRate = 60;
    }

    void FixedUpdate()
    {
        CheckWheelStatus();
    }

    public void PlayerCarMovement()
    {
        moveForce += transform.forward * speed * _Joystick.Vertical * Time.deltaTime;
        transform.position += moveForce * Time.deltaTime;
        
        moveForce *= Drag;
        moveForce = Vector3.ClampMagnitude(moveForce, maxSpeed);
        
        steerInput = _Joystick.Horizontal;
        transform.Rotate(Vector3.up * steerInput * moveForce.magnitude * steerAngle * Time.deltaTime);
        
        Debug.DrawRay(transform.position, moveForce.normalized*5);
        Debug.DrawRay(transform.position, transform.forward * 5, Color.blue);
        moveForce = Vector3.Lerp(moveForce.normalized , transform.forward , traction * Time.deltaTime) * moveForce.magnitude;

        
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

    private void CheckWheelStatus()
    {
        if (frontLeft.isGrounded || frontRight.isGrounded || backLeft.isGrounded || backRight.isGrounded)
        {
            PlayerCarMovement();
        }
        if (backLeft.isGrounded || backRight.isGrounded)
        {
            foreach (var _skidMarks in skidMarks)
            {
                _skidMarks.enabled = true;
            }
        }
        else if(!backLeft.isGrounded|| !backRight.isGrounded)
        {
            foreach (var _skidMarks in skidMarks)
            {
                _skidMarks.enabled = false;
            }
        }
    }
    public void TakeDamage(int damage)
    {
        if(!isAlive) return;
        playerHealth -= damage;
           
        CheckDamageParticles();
           
        if (playerHealth <= 0)
        {
            isAlive = false;
            Crash();
        }
    }
    private void CheckDamageParticles()
    {
        if (playerHealth == 10)
        {
            _particleSystemSmoke.gameObject.SetActive(true);
            _particleSystemSmoke.Play();
        }
        else if (playerHealth == 2)
        {
            _particleSystemFire.gameObject.SetActive(true);
            _particleSystemFire.Play();
            burningSound.Play();
        }
    }
    private void Crash()
    {
        maxSpeed = 0;
        speed = 0;
        explosionSound.Play();
        _particleSystemExplosion.gameObject.SetActive(true);
        _particleSystemExplosion.Play();
        _particleSystemSmoke.gameObject.SetActive(false);
        _particleSystemSmoke.Play();
        _particleSystemFire.gameObject.SetActive(false);
        _particleSystemFire.Play();
    }

    private bool isVehicleTrigger = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CheckPoint"))
        {
            if(isVehicleTrigger) return;
            isVehicleTrigger = true;
            Timer.Instance.StartTimer(5);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("CheckPoint"))
        {
            if(!isVehicleTrigger) return;
            isVehicleTrigger = false;
            Timer.Instance.ResetTimer();
        }
        
    }
    
    
}



// if (!frontLeft.isGrounded && !frontRight.isGrounded && !backLeft.isGrounded && !backRight.isGrounded)
// {
//     StartCoroutine(FixPlayerRotation());
// }
// else if(!frontLeft.isGrounded && !backLeft.isGrounded)
// {
//     StartCoroutine(FixPlayerRotation());
//
// }
// else if(!frontRight.isGrounded && !backRight.isGrounded)
// {
//     StartCoroutine(FixPlayerRotation());
//
// }



// IEnumerator FixPlayerRotation()
// {
//     yield return new WaitForSeconds(3f);
//     transform.rotation = Quaternion.Euler(0,0,0);
// }


