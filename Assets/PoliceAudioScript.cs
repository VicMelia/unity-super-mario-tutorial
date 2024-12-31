using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoliceAudioScript : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip poli1;
    public AudioClip poli2;
    bool audio1;
    bool audio2;

    float time = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;

        if(time >25f && !audio1)
        {
            audio1 = true;
            audioSource.clip = poli1;
            audioSource.Play();
        }

        if(time > 55f && !audio2)
        {
            audio2 = true;
            audioSource.clip = poli2;
            audioSource.Play();
        }
    }
}
