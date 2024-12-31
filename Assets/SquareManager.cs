using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareManager : MonoBehaviour
{
    public NarratorManager nm;
    public int pruebaActual = 1;
    public float fadeDuration = 1f;
    public GameObject[] prueba1;
    public GameObject[] prueba2;
    public GameObject[] prueba3;
    public enum State {Intro, Playing }

    public Camera mainCamera; // Assign the Camera in the Inspector
    public float zoomOutSize = 14f; // Target size for zoom-out
    public float zoomInSize = 5f; // Original size to zoom back to
    public float zoomDuration = 2f; // Duration for zoom in/out
    public float holdDuration = 3f; // Time to stay zoomed out

    //prueba 2
    public int contadorCirculos = 0;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator SpawnThings(GameObject[] objects)
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            foreach (GameObject obj in objects)
            {
                SpriteRenderer sprite = obj.GetComponent<SpriteRenderer>();
                if (sprite != null)
                {
                    // Get the current color
                    Color color = sprite.color;

                    // Lerp the alpha value
                    color.a = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);

                    // Apply the updated color back to the SpriteRenderer
                    sprite.color = color;
                }
            }

            elapsedTime += Time.deltaTime;

            yield return null; // Wait for the next frame
        }

        // Ensure final alpha is set to 1 after the loop
        foreach (GameObject obj in objects)
        {
            SpriteRenderer sprite = obj.GetComponent<SpriteRenderer>();
            if (sprite != null)
            {
                Color color = sprite.color;
                color.a = 1f; // Set alpha to fully visible
                sprite.color = color;
            }
        }

        nm.prueba1Started = true;
    }

    public IEnumerator DespawnThings(GameObject[] objects)
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            foreach (GameObject obj in objects)
            {
                SpriteRenderer sprite = obj.GetComponent<SpriteRenderer>();
                if (sprite != null)
                {
                    // Get the current color
                    Color color = sprite.color;

                    // Lerp the alpha value
                    color.a = Mathf.InverseLerp(1f, 0f, elapsedTime / fadeDuration);

                    // Apply the updated color back to the SpriteRenderer
                    sprite.color = color;
                }
            }

            elapsedTime += Time.deltaTime;

            yield return null; // Wait for the next frame
        }

        // Ensure final alpha is set to 1 after the loop
        foreach (GameObject obj in objects)
        {
            SpriteRenderer sprite = obj.GetComponent<SpriteRenderer>();
            if (sprite != null)
            {
                Color color = sprite.color;
                color.a = 0f; // Set alpha to fully visible
                sprite.color = color;
                obj.SetActive(false);
            }
        }
    }

    public IEnumerator CameraMovement()
    {
        // Cache the original size
        float originalSize = mainCamera.orthographicSize;

        // Smoothly zoom out
        yield return StartCoroutine(SmoothZoom(originalSize, zoomOutSize, zoomDuration));

        // Hold the zoom-out for the specified duration
        yield return new WaitForSeconds(holdDuration);

        // Smoothly zoom back in
        yield return StartCoroutine(SmoothZoom(zoomOutSize, zoomInSize, zoomDuration));

        nm.actualClip++;
        nm.NextSequence(nm.actualClip);
    }

    private IEnumerator SmoothZoom(float startSize, float endSize, float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            // Lerp the size
            mainCamera.orthographicSize = Mathf.Lerp(startSize, endSize, elapsedTime / duration);

            elapsedTime += Time.deltaTime;

            yield return null; // Wait for the next frame
        }

        // Ensure the final size is set
        mainCamera.orthographicSize = endSize;
    }

    
}
