using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    
    public bool TimerStarted = false;

    public float _timer = 0f;
    //the amount of time i want the blue player to spend in the death zone
    public float TimeIWantInSeconds = 3f;
    public Enemy enemy;
    public GameObject redWall0;
    public GameObject redWall1;
    public GameObject redWall2;
    public CameraShake cameraShake;
    public float n = 10f;
    public bool BluePlayerLostStock = false;
    
    
    // Update is called once per frame
    void Update()
    {
        //Debug.Log(_timer);
        if (TimerStarted)
        {
            _timer += Time.deltaTime;
            if (_timer > TimeIWantInSeconds)
            {
                redWall2.SetActive(true);
                //StartCoroutine(cameraShake.Shake(.3f, .5f));

                //Debug.Log(_timer);
                BluePlayerLostStock = true;
                
                
            }
        }
        else {
            ResetTimer();
        }

        //animations for deathzone
        if (_timer > 1f)
        {
            redWall0.SetActive(true);
        }
        if (_timer > 2f)
        {
            redWall1.SetActive(true);
        }
        
        if (_timer < 1f)
        {
            FadeWalls();
            
        }

    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        enemy = hitInfo.GetComponent<Enemy>();
        if (enemy != null)
        {
            if (TimerStarted) TimerStarted = false;

        }



    }
    void OnTriggerExit2D(Collider2D hitInfo)
    {
        enemy = hitInfo.GetComponent<Enemy>();
        if (enemy != null)
        {
            if (!TimerStarted) TimerStarted = true;
        }


    }
    
    void FadeWalls()
    {
        for (float i = 0f; i < 10; i++)
        {
            
        }
        redWall0.SetActive(false);
        redWall1.SetActive(false);
        redWall2.SetActive(false);
    }
    public void ResetTimer()
    {
        _timer = 0f;
        TimerStarted = false;
    }
    
}
