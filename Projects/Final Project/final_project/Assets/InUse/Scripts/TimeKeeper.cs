using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class TimeKeeper : MonoBehaviour
{
    public float timeLeft;
    public TextMeshProUGUI timerText;
    private bool timerActive = false;
    public UnityEvent timeOver;
    public UnityEvent timeToggled;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (timerActive)
        {
            if (timeLeft > 0)
            {
                timeLeft -= Time.deltaTime;
                UpdateTimer(timeLeft);
            }
            else
            {
                EndTimer();
            }
        }
    }

    private void UpdateTimer(float currentTime)
    {
        currentTime++;

        float minutes = Mathf.Floor(currentTime / 60f);
        float seconds = Mathf.Floor(currentTime % 60f);

        timerText.text = string.Format("{0:00} : {1:00}", minutes, seconds);
    }

    public void StartTimer(float startTime)
    {
        timeLeft = startTime * 60;
        timerActive = true;
    }

    public void PauseTimer()
    {
        timerActive = false;
    }

    public void ResumeTimer()
    {
        timerActive = true;
    }

    public void EndTimer()
    {
        timerActive = false;
        Debug.Log("Time's up!");
        timeOver.Invoke();
    }
}
