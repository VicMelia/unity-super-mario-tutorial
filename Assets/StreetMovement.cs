using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StreetMovement : MonoBehaviour
{

    StreetGenerator sg;
    bool finished = false;

    public float speed = 5f;
    // Start is called before the first frame update
    void Start()
    {
        sg = GameObject.Find("Generator").GetComponent<StreetGenerator>();
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
                GameObject copy = this.gameObject;
                sg.streetList.Remove(copy);
                GameObject.Destroy(this.gameObject);
            }
        }
        
    }
}
