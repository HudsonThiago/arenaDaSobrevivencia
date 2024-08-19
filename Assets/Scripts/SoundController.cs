using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{

    public static SoundController instance;
    [SerializeField] private List<AudioSource> source;

    void Awake()
    {
        instance = this;
    }

    public void echoSound(int index)
    {
        source[index].Play();
    }

}
