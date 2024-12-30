using UnityEngine;

public class LookScript : MonoBehaviour
{
    public Transform player;
    public bool rotating = true;

    // Update is called once per frame
    void Update()
    {
        if (rotating)
        {
            // Calculate the direction to the player
            Vector3 direction = player.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            // Rotate only on the Z-axis
            transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }
        
    }
}
