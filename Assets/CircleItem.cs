using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleItem : MonoBehaviour
{
    public SquareManager sm;
    public NarratorManager nm;
    bool clicked = false;

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

            clicked = true;
            nm.audio.clip = nm.AudioCirculos[sm.contadorCirculos];
            nm.audio.Play();
            sm.contadorCirculos++;
            if (sm.contadorCirculos == 7)
            {
                StartCoroutine(sm.CameraMovement());
            }


        }

    }
}
