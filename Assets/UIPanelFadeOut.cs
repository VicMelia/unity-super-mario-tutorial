using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPanelFadeOut : MonoBehaviour
{
    public CanvasGroup canvasGroup; // Reference to the CanvasGroup component
    public float fadeDuration = 1f; // Duration of the fade-in effect

    void Start()
    {
        // Ensure the CanvasGroup component is set
        if (canvasGroup == null)
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        // Start with the panel fully transparent
        canvasGroup.alpha = 1f;

        // Disable interaction during the fade-in
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        // Start the fade-in effect
        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            // Increase alpha over time
            canvasGroup.alpha = Mathf.InverseLerp(1f, 0f, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;

            yield return null; // Wait for the next frame
        }

        // Ensure the final alpha is fully visible
        canvasGroup.alpha = 0f;

    
    }
}
