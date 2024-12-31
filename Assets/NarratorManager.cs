using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NarratorManager : MonoBehaviour
{
    public SquareManager sm; //change state of play

    public AudioSource audio;
    public AudioClip[] clips;
    public int actualClip;
    public AudioClip[] AudioCirculos;

    public SquareItem cuadradoEspecial;
    public CircleItem circuloEspecial;
    public GameObject cuadrados;
    public GameObject circulos;

    public AudioClip cuentaAtras;
    public AudioClip fraseFinal;
    public UIPanelFadeIn UIPanelFadeIn;

    public bool prueba1Started;

    // Start is called before the first frame update
    void Start()
    {

        StartCoroutine(PlaySequence(actualClip));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NextSequence(int actualClip)
    {
        StartCoroutine(PlaySequence(actualClip));
    }

    IEnumerator PlaySequence(int actualClip)
    {
        sm.pruebaActual++;
        audio.clip = clips[actualClip];
        audio.Play();
        yield return new WaitForSeconds(clips[actualClip].length);
        if (sm.pruebaActual == 1) StartCoroutine(sm.SpawnThings(sm.prueba1));
        else if (sm.pruebaActual == 2) {
            StartCoroutine(sm.DespawnThings(sm.prueba1));
            circuloEspecial.gameObject.SetActive(true);
            cuadradoEspecial.gameObject.SetActive(false);
            cuadrados.SetActive(false);
            circulos.SetActive(true);
            StartCoroutine(sm.SpawnThings(sm.prueba2));
  
        } 
        else if(sm.pruebaActual == 3)
        {
            StartCoroutine(sm.DespawnThings(sm.prueba2));
            circuloEspecial.enabled = false;
            StartCoroutine(sm.SpawnThings(sm.prueba3));
            StartCoroutine(CountDownFinal());
        }
    }

    IEnumerator CountDownFinal()
    {
        audio.clip = cuentaAtras;
        audio.Play();
        yield return new WaitForSeconds(cuentaAtras.length);
        audio.clip = fraseFinal;
        audio.Play();
        yield return new WaitForSeconds(fraseFinal.length);
        UIPanelFadeIn.StartCoroutine(UIPanelFadeIn.FadeIn(true));
    }

    
}
