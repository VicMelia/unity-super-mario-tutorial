using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIPanelFadeIn : MonoBehaviour
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
        //canvasGroup.alpha = 0f;

        // Disable interaction during the fade-in
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        // Start the fade-in effect
        //StartCoroutine(FadeIn());
    }

    public IEnumerator FadeIn(bool win)
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            // Increase alpha over time
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;

            yield return null; // Wait for the next frame
        }

        // Ensure the final alpha is fully visible
        canvasGroup.alpha = 1f;
        NextStage(win);


    }

    public void NextStage(bool win)
    {
        if(!win) // Restart the scene
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        else
        {
            // Load the next scene based on the build index
            int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

            // Check if the next scene index is within the valid range
            if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
            {
                SceneManager.LoadScene(nextSceneIndex);
            }
            else
            {
                Debug.LogWarning("No more scenes in build settings. Ensure you've added all required scenes.");
            }
        }
    }


}
