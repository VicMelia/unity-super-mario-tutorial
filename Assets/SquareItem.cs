using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareItem : MonoBehaviour
{
    public SquareManager sm;
    public NarratorManager nm;
    public bool isCorrect = false;
    bool clicked = false;

    public AudioClip clip;

    // Start is called before the first frame update
    void Start()
    {
        nm = GameObject.Find("Narrator").GetComponent<NarratorManager>();
        sm = GameObject.Find("SquareManager").GetComponent<SquareManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseDown()
    {
        if (!clicked)
        {


            nm.audio.clip = clip;
            nm.audio.Play();
            clicked = true;
            if (isCorrect)
            {
                StartCoroutine(sm.CameraMovement());
            }

            
        }
        
    }


}
