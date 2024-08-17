using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Timeline;

public class Timer : MonoBehaviour
{
    [Header("Timer Settings")]
    [SerializeField] private TMP_Text text;
    [SerializeField] private static float time;
    private bool timeControll = true;
    // Start is called before the first frame update
    void Start()
    {
        time = 0;
        InvokeRepeating("displayTimer", 1f, 1f);
        
    }

    private void Update()
    {
        if (timeControll && time == 10*60)
        {
            timeControll = false;
            AttributeSystem.instance.victory();
        }
    }

    public void displayTimer()
    {
        if (time < 0f) time = 0f;
        time++;
        float minutes = Mathf.FloorToInt(time / 60);
        float seconds = Mathf.FloorToInt(time % 60);
        text.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public static float getTimer()
    {
        return time;
    }

    public static string getTimerToString()
    {
        float minutes = Mathf.FloorToInt(time / 60);
        float seconds = Mathf.FloorToInt(time % 60);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
