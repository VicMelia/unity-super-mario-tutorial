using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    SpriteRenderer sp;
    public Sprite[] sprites;

    // Start is called before the first frame update
    void Start()
    {
        sp.sprite = sprites[Random.Range(0, sprites.Length)];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && collision.gameObject.TryGetComponent(out Player player))
        {
            
            player.Death2();
            GameObject.Destroy(this.gameObject);

        }

    }
}
