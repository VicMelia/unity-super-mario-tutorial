using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinematicInteraction : MonoBehaviour
{

    bool actived;
    public Transform player;
    public CinematicManager manager;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!actived)
        {
            float d = Vector2.Distance(transform.position, player.position);
            {
                if(d < 3f)
                {
                    actived = true;
                    manager.StartCoroutine(manager.StartCinematic());

                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !actived)
        {
            actived = true;
            manager.StartCoroutine(manager.StartCinematic());
        }
    }

  
}
