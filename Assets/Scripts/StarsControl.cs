using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarsControl : MonoBehaviour
{
    [SerializeField] private List<GameObject> Stars;
    [SerializeField] private float gameTime=0;
    void Start()
    {
        
    }
    void Update()
    {
        CriminalDegree();
    }

    private void CriminalDegree()
    {
        gameTime = Time.time;
        if (gameTime > 10)
        {
            Stars[1].SetActive(true);
        }
        if(gameTime > 20)
        {
            Stars[2].SetActive(true);
        }
        if(gameTime > 35)
        {
            Stars[3].SetActive(true);
        }
        if(gameTime > 40)
        {
            Stars[4].SetActive(true);
        }
    }
}
