using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager AM;
    public AudioClip song1;
    public AudioClip song2;
    public AudioSource source;

    private void Start()
    {
        AudioManager.AM = this;
    }

    public void ChangeSong(int id)
    {
        if (id == 0)
            source.clip = song1;
        if(id == 1)
            source.clip = song2;
    }
}
