using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnderPearl : MonoBehaviour
{
    // Start is called before the first frame update

    bool finished = false;
    public float speed = 5f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!finished)
        {
            transform.Translate(Vector3.up * speed * Time.deltaTime);
            if (transform.position.y >= 20f)
            {
                finished = true;
                GameObject.Destroy(this.gameObject);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Police"))
        {
            GameObject.Destroy(collision.gameObject);
            GameObject.Destroy(this.gameObject);

        }
    }
}
