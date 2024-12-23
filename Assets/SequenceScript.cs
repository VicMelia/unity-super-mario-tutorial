using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequenceScript : MonoBehaviour
{
    public AudioSource audioSource; // Referencia al AudioSource en el objeto
    public AudioClip[] audioClips;  // Array de clips de audio a reproducir
    public MinigameManager gm; // Elementos del juego a activar al finalizar

    private int currentClipIndex = 0; // Índice del clip actual

    void Start()
    {
        
        // Iniciar la secuencia de reproducción
        StartCoroutine(PlayAudioSequence());
    }

    IEnumerator PlayAudioSequence()
    {
        while (currentClipIndex < audioClips.Length)
        {
            // Configurar y reproducir el clip actual
            audioSource.clip = audioClips[currentClipIndex];
            audioSource.Play();

            // Esperar a que termine de reproducirse
            yield return new WaitForSeconds(audioSource.clip.length);

            // Pasar al siguiente clip
            currentClipIndex++;
        }

        // Cuando todos los clips hayan terminado, iniciar el juego
        StartGame();
    }

    void StartGame()
    {
        gm.StartGame();
    }
}
