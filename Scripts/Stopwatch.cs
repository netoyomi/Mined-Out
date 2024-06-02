using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Stopwatch : MonoBehaviour
{
    private Text stopwatchText;

    public float elapsedTime = 0f;
    private bool isRunning = true;
    private void Start()
    {
        stopwatchText = GetComponent<Text>();
    }
    void Update()
    {
        if (isRunning)
        {
            elapsedTime += Time.deltaTime;
            UpdateTimeDisplay();
        }
    }

    void UpdateTimeDisplay()
    {
        int minutes = Mathf.FloorToInt(elapsedTime / 60f);
        int seconds = Mathf.FloorToInt(elapsedTime % 60f);
      

        stopwatchText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
    public void StopStopwatch()
    {
        isRunning = false;
    }
    public void ResetStopwatch()
    {
        elapsedTime = 0f;
        isRunning = true;
    }
}
