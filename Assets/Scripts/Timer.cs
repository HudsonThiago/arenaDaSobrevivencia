using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [Header("Timer Settings")]
    [SerializeField] private TMP_Text text;
    [SerializeField] private static float time;
    // Start is called before the first frame update
    void Start()
    {
        time = 0;
        InvokeRepeating("displayTimer", 1f, 1f);
        
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
}
