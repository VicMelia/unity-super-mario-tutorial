using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ArrowMover : MonoBehaviour
{
    private float speed;
    public KeyCode keyToPress;      // Key corresponding to this hit zone
    public float hitTolerance = 1f; // Tolerance for a "perfect" hit
    public Transform hitZone;
    public bool finished;
    MinigameManager mg;

    private SpriteRenderer spriteRenderer; // Reference to the arrow's SpriteRenderer

    private void Start()
    {
        hitZone = GameObject.Find("SPAWNHEIGHT").transform;
        spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        mg = GameObject.Find("GameManager").GetComponent<MinigameManager>();
    }

    public void SetSpeed(float arrowSpeed)
    {
        speed = arrowSpeed;
    }

    void Update()
    {
        // Move the arrow downward
        transform.Translate(Vector3.down * speed * Time.deltaTime);

        // Destroy the arrow if it goes off-screen
        if (!finished && transform.position.y < -Screen.height)
        {
            finished = true;
            Debug.Log("Miss!");
            mg.fallos++;
            Destroy(gameObject);
        }

        if (!finished && Input.GetKeyDown(keyToPress))
        {
            float distanceToZone = Mathf.Abs(transform.position.y - hitZone.position.y);

            if (distanceToZone > hitTolerance + 2f) return;
            else if (distanceToZone <= hitTolerance)
            {
                SetSpeed(0f);
                finished = true;
                Debug.Log("Hit!");
                
                // Change color to green and start fade-out
                spriteRenderer.color = Color.green;
                mg.aciertos++;
                StartCoroutine(FadeOutAndDestroy());
            }

            else
            {
                finished = true;
                // No valid hit
                Debug.Log("Miss!");
                spriteRenderer.color = Color.red;
                mg.fallos++;
                StartCoroutine(FadeOutAndDestroy());
            }
        }

        
    }

    public IEnumerator FadeOutAndDestroy()
    {
        float fadeDuration = 0.5f; // Duration of the fade-out effect
        float elapsedTime = 0f;

        Color initialColor = spriteRenderer.color;
        while (elapsedTime < fadeDuration)
        {
            // Gradually reduce the alpha of the sprite
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            spriteRenderer.color = new Color(initialColor.r, initialColor.g, initialColor.b, alpha);
            elapsedTime += Time.deltaTime;

            yield return null; // Wait for the next frame
        }

        // Ensure the alpha is completely zero
        spriteRenderer.color = new Color(initialColor.r, initialColor.g, initialColor.b, 0f);

        // Destroy the arrow object
        Destroy(gameObject);
    }
}

