using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CarsManager : MonoBehaviour
{
    public PoliceController _PoliceController;
    public CinemachineVirtualCamera cam;
    public List<GameObject> cars = new List<GameObject>();
    private int carNo = 0;
    
    internal void SelectCar()
    {
        if(carNo == cars.Count-1) return;
        
        cars[carNo].SetActive(false);
        carNo++;
        cars[carNo].SetActive(true);
        _PoliceController._target = cars[carNo];
        cam.Follow = cars[carNo].transform;
    }
}
