using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomAudio : MonoBehaviour
{

    public AudioSource audioSource;
    public AudioClip[] clips;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayRandomAudio()
    {
        audioSource.clip = clips[UnityEngine.Random.Range(0, clips.Length)];
        audioSource.Play();
    }
}
