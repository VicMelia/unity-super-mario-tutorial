using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Police : MonoBehaviour
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
            transform.Translate(Vector3.down * speed * Time.deltaTime);
            if (transform.position.y <= -5.73f)
            {
                finished = true;
                GameObject.Destroy(this.gameObject);
            }
        }
    }
}
