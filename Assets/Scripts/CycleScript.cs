using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CycleScript : MonoBehaviour
{

    public float timerLimit = 20;
    public int cycleCount;
    private float timeRemaining;
    public bool timerIsRunning = false;
    [SerializeField]
    public TMP_Text timeText;

    private void Start()
    {
        timeRemaining = timerLimit;
        timerIsRunning = true;
    }

    void Update()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                    DisplayTime(timeRemaining);
                    timeRemaining -= Time.deltaTime;
            }
            else
            {
                    Debug.Log("Time has ran out");
                    timeRemaining = 0;
                    timerIsRunning = false;
                    CycleChange();
            }
        }
    }

    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;

        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void CycleChange()
    {
        timeRemaining = timerLimit;
        timerIsRunning = true;
        cycleCount += 1;
    }
}
