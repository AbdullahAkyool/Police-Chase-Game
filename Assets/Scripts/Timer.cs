using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public static Timer Instance;
    public CarsManager CarsManager;
    public TextMeshPro timerText;
    
    public float time = 0F;

    public float endDuration;
    private bool isTimerStarted = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Update()
    {
        Tick();
        UpdateTimerText();
    }

    private void UpdateTimerText()
    {
        timerText.text = time.ToString("0.0");
    }

    private void Tick()
    {
        if(!isTimerStarted) return;

        if (time >= endDuration)
        {
            //burada işlem yapılacak
            CarsManager.SelectCar();
            ResetTimer();
        }
        
        time += Time.deltaTime;
    }

    public void StartTimer(float duration)
    {
        isTimerStarted = true;
        endDuration = duration;
    }
    
    public void StopTimer()
    {
        isTimerStarted = false;
    }

    public void ResetTimer()
    {
        isTimerStarted = false;
        time = 0F;
    }
}
