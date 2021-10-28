using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance = null;
    public AudioSource[] audios;
    void Start()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    public void bowSound()
    {
        if(!audios[0].isPlaying)
            audios[0].Play();
    }
    public void deathSound()
    {
        if (!audios[1].isPlaying)
            audios[1].Play();
       
    }

    public void warHorn()
    {
        if (!audios[2].isPlaying)
            audios[2].Play();
    }
}
