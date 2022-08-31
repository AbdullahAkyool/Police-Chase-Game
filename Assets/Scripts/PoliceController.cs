using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PoliceController : MonoBehaviour
{
       [SerializeField] private WheelCollider frontRight;
       [SerializeField] private WheelCollider frontLeft;
       [SerializeField] private WheelCollider backRight;
       [SerializeField] private WheelCollider backLeft;
   
       [SerializeField] private Transform frontRightTransform;
       [SerializeField] private Transform frontLeftTransform;
       [SerializeField] private Transform backRightTransform;
       [SerializeField] private Transform backLeftTransform;
       
       [SerializeField] private float speed;
       [SerializeField] private float maxSpeed;
       private Vector3 moveForce;
       private float Drag = .98f;
       [SerializeField] private float traction;

       public GameObject _target;
       
       private float currentTurnAngle = 0f;
       public TrailRenderer[] skidMarks;

       public int policeHealth;
       public bool isAlive = true;

       public GameObject[] redLights;
       public GameObject[] blueLights;

       [SerializeField] private PlayerController _playerController;
       [SerializeField] private ParticleSystem _particleSystemSmoke;
       [SerializeField] private ParticleSystem _particleSystemFire;
       [SerializeField] private ParticleSystem _particleSystemExplosion;
       [SerializeField] private AudioSource sirenSound;
       [SerializeField] private AudioSource burningSound;
       [SerializeField] private AudioSource explosionSound;
       
       void Start()
       {
           _target = FindObjectOfType<PlayerController>().gameObject;
           _playerController = FindObjectOfType<PlayerController>();
           StartCoroutine(SirenSystem());
       }

       void FixedUpdate()
       {
           CheckWheelStatus();
       }
    

       private void CheckWheelStatus()
       {
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

           if (frontLeft.isGrounded || frontRight.isGrounded || backLeft.isGrounded || backRight.isGrounded)
           {
               PoliceCarMovement();
           }
       }
       public void PoliceCarMovement()
       {
           Vector3 pointTarget = _target.transform.position - transform.position;
           
           moveForce += transform.forward * speed * pointTarget.magnitude * Time.deltaTime;
           transform.position += moveForce * Time.deltaTime;
           moveForce *= Drag;
           moveForce = Vector3.ClampMagnitude(moveForce, maxSpeed);
           // steerInput = _Joystick.Horizontal;
           // transform.Rotate(Vector3.up * steerInput * moveForce.magnitude * steerAngle * Time.deltaTime);
           
           RotateToTarget(pointTarget);
           moveForce = Vector3.Lerp(moveForce.normalized , transform.forward , traction * Time.deltaTime) * moveForce.magnitude;

           frontLeft.steerAngle = currentTurnAngle;
           frontRight.steerAngle = currentTurnAngle;
           
           UpdateWheel(frontRight,frontRightTransform);
           UpdateWheel(frontLeft,frontLeftTransform);
           UpdateWheel(backRight,backRightTransform);
           UpdateWheel(backLeft,backLeftTransform);
       }

       private void RotateToTarget(Vector3 pointTarget)
       {
           if(!isAlive) return;
           transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(pointTarget), 3*Time.deltaTime);
       }
       
       void UpdateWheel(WheelCollider wheelcol, Transform trans)
       {
           Vector3 position;
           Quaternion rotation;
           wheelcol.GetWorldPose(out position, out rotation);
           trans.position = position;
           trans.rotation = rotation;
       }
       
       
       private void TakeDamage(int damage)
       {
           if(!isAlive) return;
           policeHealth -= damage;
           
           CheckDamageParticles();
           
           if (policeHealth <= 0)
           {
               isAlive = false;
               Crash();
           }
       }
       private void CheckDamageParticles()
       {
           if (policeHealth == 3)
           {
               _particleSystemSmoke.gameObject.SetActive(true);
               _particleSystemSmoke.Play();
           }
           else if (policeHealth == 1)
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
           sirenSound.Stop();
           explosionSound.Play();
           _particleSystemExplosion.gameObject.SetActive(true);
           _particleSystemExplosion.Play();
       }

       IEnumerator SirenSystem()
       {
           yield return new WaitForSeconds(.2f);
           foreach (var _redLights in redLights)
           {
               _redLights.SetActive(true);
           }
           foreach (var _blueLights in blueLights)
           {
               _blueLights.SetActive(false);
               
           }
           
           yield return new WaitForSeconds(.2f);
           foreach (var _redLights in redLights)
           {
               _redLights.SetActive(false);
           }
           foreach (var _blueLights in blueLights)
           {
               _blueLights.SetActive(true);
           }
           StartCoroutine(SirenSystem());
       }
       private void OnCollisionEnter(Collision collision)
       {
           if (collision.gameObject.tag == _target.tag)
           {
              TakeDamage(1);
              _playerController.TakeDamage(1);
           }
       }
}

// Vector3 pointTarget = transform.position - _target.transform.position;
// pointTarget.y = 0;
// rb.velocity = transform.forward * speed;
// float value = Vector3.Cross(pointTarget, transform.forward).y;
//
// rb.angularVelocity = steerAngle * value * new Vector3(0, 1, 0);



//transform.rotation = Quaternion.Slerp(transform.rotation , Quaternion.LookRotation(_target.transform.position - transform.position), 1* Time.deltaTime);
//transform.position += _target.transform.forward * speed * Time.deltaTime;


// Vector3 pointTarget = transform.position - _target.transform.position;
//
//
// moveForce += transform.forward * speed * _Joystick.Vertical * Time.deltaTime;
// transform.position += moveForce * Time.deltaTime;
//
// moveForce *= Drag;
// moveForce = Vector3.ClampMagnitude(moveForce, maxSpeed);
//
// steerInput = _Joystick.Horizontal;
// transform.Rotate(Vector3.up * steerInput * moveForce.magnitude * steerAngle * Time.deltaTime);
//
// Debug.DrawRay(transform.position, moveForce.normalized*5);
// Debug.DrawRay(transform.position, transform.forward * 5, Color.blue);
// moveForce = Vector3.Lerp(moveForce.normalized , transform.forward , traction * Time.deltaTime) * moveForce.magnitude;
//
// currentTurnAngle = maxTurnAngle * _Joystick.Horizontal;
