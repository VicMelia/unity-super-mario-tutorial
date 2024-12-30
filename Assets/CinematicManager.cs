using System.Collections;
using UnityEngine;

public class CinematicManager : MonoBehaviour
{
    public AudioSource audio;
    public AudioClip audio1;
    public AudioClip audio2;

    public PlayerMovement player;
    public SpriteRenderer sp;

    public AmhaScript amha;
    public LookScript ls;

    public SideScrollingCamera cam;

    public IEnumerator StartCinematic()
    {
        // Disable player control
        cam.actived = false;
        player.velocity = Vector3.zero;
        player.inputAxis = 0;
        player.enabled = false;

        // Play the first audio clip
        audio.clip = audio1;
        audio.Play();

        // Wait for the first audio to finish playing
        yield return new WaitForSeconds(audio1.length + 1f);
        audio.pitch = 0.95f;

        // Lerp the sprite alpha from 0 to 1 over 1 second
        float lerpDuration = 1f;
        float startAlpha = 0f;
        float endAlpha = 1f;
        float elapsedTime = 0f;

        // Get the current color of the sprite
        Color spriteColor = sp.color;

        while (elapsedTime < lerpDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / lerpDuration;

            // Update only the alpha channel
            spriteColor.a = Mathf.Lerp(startAlpha, endAlpha, t);
            sp.color = spriteColor;

            yield return null; // Wait for the next frame
        }

        // Ensure the sprite alpha is set to 1
        spriteColor.a = endAlpha;
        sp.color = spriteColor;

        // Play the second audio clip
        audio.clip = audio2;
        audio.Play();

        // Optionally re-enable player control or add more logic after audio2 finishes
        yield return new WaitForSeconds(audio2.length);
        yield return new WaitForSeconds(1f);
        player.enabled = true;
        amha.enabled = true;
        amha.gameObject.GetComponent<CircleCollider2D>().enabled = true;
        ls.enabled = true;
    }
}
