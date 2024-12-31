using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MinigameManager : MonoBehaviour
{
    public UIPanelFadeIn UIPanelFadeIn;
    public CanvasGroup trivialGroup;
    public GameObject[] arrowPrefabs; // Array of arrow prefabs (Up, Down, Left, Right)
    public Transform[] spawnPoints;  // Spawn points corresponding to lanes
    public float spawnRate = 1f;     // How often arrows are spawned
    public float arrowSpeed = 2f;    // Speed of falling arrows
    public AudioSource audioSource; // Reference to the AudioSource for the music or audio
    public AudioClip song;

    private bool isGameRunning = false; // Tracks if the game is running
    private Coroutine spawnRoutine; // Reference to the spawn coroutine

    public int aciertos = 0;
    public int fallos = 0;

    public AudioClip questionAudio; // Audio clip for completion
    public AudioClip restartGameAudio;  // Audio clip for restart
    public AudioClip final1;  // Audio clip for restart
    public AudioClip final2;  // Audio clip for restart

    public AudioSource instrumentalFondo;

    // Start the game when this method is called
    public void StartGame()
    {
        if (audioSource == null || audioSource.clip == null)
        {
            Debug.LogError("AudioSource or AudioClip is not assigned!");
            return;
        }

        audioSource.clip = song;

        // Start playing the audio
        instrumentalFondo.Play();


        StartCoroutine(StartDelaySong());

        // Start spawning arrows
        isGameRunning = true;
        spawnRoutine = StartCoroutine(SpawnArrows());

       
    }

    IEnumerator SpawnArrows()
    {
        while (isGameRunning)
        {
            // Choose a random lane
            int randomIndex = Random.Range(0, spawnPoints.Length);

            // Instantiate the arrow at the chosen lane
            GameObject arrow = Instantiate(arrowPrefabs[randomIndex], spawnPoints[randomIndex].position, Quaternion.identity);
            

            // Add a script to make the arrow move
            arrow.GetComponent<ArrowMover>().SetSpeed(arrowSpeed);

            // Wait before spawning the next arrow
            yield return new WaitForSeconds(spawnRate);
        }
    }

    IEnumerator StartDelaySong()
    {
        yield return new WaitForSeconds(0.25f);
        audioSource.Play();
        // Stop the game when the audio is finished
        StartCoroutine(StopGameWhenAudioEnds());

    }

    IEnumerator StopGameWhenAudioEnds()
    {
        // Wait until the audio finishes
        while (audioSource.isPlaying)
        {
            yield return null; // Wait for the next frame
        }

        // Stop the game
        isGameRunning = false;

        // Stop spawning arrows
        if (spawnRoutine != null)
        {
            StopCoroutine(spawnRoutine);
        }

        // Optionally, clean up remaining arrows
        CleanupArrows();
        instrumentalFondo.Stop();

        if (fallos > 7) RestartGame();
        else NextStep();



        Debug.Log("Game Over! Audio finished playing.");
    }

    void CleanupArrows()
    {
        // Find and destroy all remaining arrows
        ArrowMover[] arrows = FindObjectsOfType<ArrowMover>();
        foreach (ArrowMover arrow in arrows)
        {
            arrow.SetSpeed(0f);
            arrow.StartCoroutine(arrow.FadeOutAndDestroy());
        }
    }

    void RestartGame()
    {
        StartCoroutine(RestartGameSequence());
    }

    void NextStep()
    {
        StartCoroutine(TrivialSequence());
    }

    public void EmpezarFinalBueno()
    {
        trivialGroup.alpha = 0f;
        StartCoroutine(FinalBueno());
    }

    public void EmpezarFinalMalo()
    {
        trivialGroup.alpha = 0f;
        StartCoroutine(FinalMalo());
    }


    IEnumerator RestartGameSequence()
    {
        // Play the restart audio
        if (restartGameAudio != null)
        {
            audioSource.clip = restartGameAudio;
            audioSource.Play();

            // Wait for the audio to finish
            while (audioSource.isPlaying)
            {
                yield return null;
            }
        }

        UIPanelFadeIn.StartCoroutine(UIPanelFadeIn.FadeIn(false));

    }

    private IEnumerator TrivialSequence()
    {
        // Play the restart audio
        if (questionAudio != null)
        {
            audioSource.clip = questionAudio;
            audioSource.Play();

            // Wait for the audio to finish
            while (audioSource.isPlaying)
            {
                yield return null;
            }
        }

        trivialGroup.gameObject.SetActive(true);
        trivialGroup.alpha = 1f;

        

    }

    public IEnumerator FinalBueno()
    {
        // Play the restart audio
        if (final1 != null)
        {
            audioSource.clip = final1;
            audioSource.Play();

            // Wait for the audio to finish
            while (audioSource.isPlaying)
            {
                yield return null;
            }
        }

        UIPanelFadeIn.StartCoroutine(UIPanelFadeIn.FadeIn(true));
    }

    public IEnumerator FinalMalo()
    {
        // Play the restart audio
        if (final2 != null)
        {
            audioSource.clip = final2;
            audioSource.Play();

            // Wait for the audio to finish
            while (audioSource.isPlaying)
            {
                yield return null;
            }
        }

        UIPanelFadeIn.StartCoroutine(UIPanelFadeIn.FadeIn(true));
    }
}
