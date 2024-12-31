using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarPlayer : MonoBehaviour
{
    public int actualPosition = 1;
    public GameObject enderPearl;

    public bool hasPearl = false;

    public AudioSource enderAudio;
    public AudioClip[] enderClip;
    public AudioClip crashClip;

    public CanvasGroup fadePanel;
    public UIPanelFadeIn fadeInPanel;

    public AudioSource music;
    public bool dead = false;
    public bool win;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (hasPearl && Input.GetKeyDown(KeyCode.Space))
        {
            hasPearl = false;
            Instantiate(enderPearl, new Vector2(transform.position.x, transform.position.y + 0.3f), Quaternion.identity);
            enderAudio.clip = enderClip[Random.Range(0, enderClip.Length)];
            enderAudio.Play();

        }
        if(actualPosition > 0 && Input.GetKeyDown(KeyCode.LeftArrow))
        {
            actualPosition--;
            transform.position = new Vector2(transform.position.x - 1f, -1.367468f);

        }

        if(actualPosition < 2 && Input.GetKeyDown(KeyCode.RightArrow))
        {
            actualPosition++;
            transform.position = new Vector2(transform.position.x + 1f, -1.367468f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Item"))
        {
            hasPearl = true;
            GameObject.Destroy(collision.gameObject);
        }

        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!dead && collision.gameObject.CompareTag("Police") && !win)
        {
            dead = true;
            music.Stop();
            enderAudio.clip = crashClip;
            enderAudio.Play();
            fadePanel.alpha = 1f;
            StartCoroutine(RestartGame());

        }
    }

    IEnumerator RestartGame()
    {
        yield return new WaitForSeconds(3f);
        fadeInPanel.NextStage(false);

    }
}
