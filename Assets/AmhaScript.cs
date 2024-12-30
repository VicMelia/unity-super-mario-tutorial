using System.Collections;
using UnityEngine;

public class AmhaScript : MonoBehaviour
{
    public Transform[] positions; // Array of positions for the enemy to move through
    public Transform lateralPosition; // The lateral position to move to before the charge
    public GameObject projectilePrefab; // The projectile to shoot
    public Transform player; // Reference to the player's transform
    public float moveSpeed = 2f; // Speed of the enemy's movement
    public float chargeSpeed = 10f; // Speed of the charge attack
    public float shootCooldown = 1f; // Time between shots
    public int loopMin = 1; // Minimum number of loops through positions
    public int loopMax = 3; // Maximum number of loops through positions
    public float curveFactor = 0.5f; // How curved the path is (higher is more curved)

    private int currentPositionIndex = 0;
    private int loopsRemaining;
    private bool isCharging = false;
    private bool isResetting = false;
    private float shootTimer = 0f;

    private Vector3 curveStart;
    private Vector3 curveEnd;
    private Vector3 curveControl;
    private float curveProgress = 0f;

    private int lives = 3; // Starting lives
    private int speedStage = 0; // To track which speed stage we're in (0, 1, 2)

    public LookScript ls;

    private void Start()
    {
        loopsRemaining = Random.Range(loopMin, loopMax + 1);
        SetNextCurve();
    }

    private void Update()
    {
        if (isCharging)
        {
            ls.rotating = false;
            ChargeAttack();
        }
        else if (isResetting)
        {
            ResetBehavior();
        }
        else
        {
            PatrolAndShoot();
        }
    }

    private void PatrolAndShoot()
    {
        // Move along the current curve
        curveProgress += moveSpeed * Time.deltaTime / Vector3.Distance(curveStart, curveEnd);
        if (curveProgress > 1f)
        {
            curveProgress = 1f;
        }

        transform.position = CalculateBezierPoint(curveProgress, curveStart, curveControl, curveEnd);

        // Check if reached the end of the curve
        if (curveProgress >= 1f)
        {
            // Shoot at the player
            if (shootTimer <= 0f)
            {
                ShootProjectile();
                shootTimer = shootCooldown;
            }

            currentPositionIndex++;
            if (currentPositionIndex >= positions.Length)
            {
                currentPositionIndex = 0;
                loopsRemaining--;

                if (loopsRemaining <= 0)
                {
                    // Move to lateral position after finishing loops
                    isResetting = true;
                    return;
                }
            }

            SetNextCurve();
        }

        shootTimer -= Time.deltaTime;
    }

    private void SetNextCurve()
    {
        curveStart = transform.position;
        curveEnd = positions[currentPositionIndex].position;

        // Create a control point to generate the curve
        Vector3 midPoint = (curveStart + curveEnd) / 2f;
        Vector3 direction = (curveEnd - curveStart).normalized;
        curveControl = midPoint + Vector3.Cross(direction, Vector3.forward) * curveFactor;

        curveProgress = 0f;
    }

    private void ResetBehavior()
    {
        // Move to the lateral position
        transform.position = Vector3.MoveTowards(transform.position, lateralPosition.position, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, lateralPosition.position) < 0.1f)
        {
            isResetting = false;
            isCharging = true;
            Invoke(nameof(PerformCharge), 3f);
        }
    }

    private void PerformCharge()
    {
        isCharging = true;
    }

    private void ChargeAttack()
    {
        // Charge attack from left to right
        transform.Translate(Vector3.right * chargeSpeed * Time.deltaTime);

        if (transform.position.x > 55f) // Adjust as needed for end of charge
        {
            isCharging = false;
            Invoke(nameof(ResetToPatrol), 2f);
        }
    }

    private void ResetToPatrol()
    {
        ls.rotating = true;
        currentPositionIndex = 0;
        // Smoothly move towards the first position in the array
        Vector3 targetPosition = positions[0].position;
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        // Check if reached the first position
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            // Reset loops and curve progression
            loopsRemaining = Random.Range(loopMin, loopMax + 1);
            currentPositionIndex = 0;
            SetNextCurve();
            isResetting = false;
        }
    }

    private void ShootProjectile()
    {
        if (projectilePrefab != null && player != null)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = direction * 4f; // Adjust speed as needed
            }
        }
    }

    private Vector3 CalculateBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        // Quadratic Bezier Curve formula
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;

        Vector3 point = (uu * p0) + (2 * u * t * p1) + (tt * p2);
        return point;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && collision.gameObject.TryGetComponent(out Player player))
        {
            if (collision.transform.DotTest(transform, Vector2.down)) //gets hit
            {
                StartCoroutine(RedFlashCoroutine());
                TakeDamage();
            }
            else
            {
                player.Death2();
            }
        }
    }

    private void TakeDamage()
    {
        lives--; // Decrease the lives

        if (lives <= 0)
        {
            // Handle enemy death
            Destroy(gameObject);
        }
        else
        {
            // Increase speed based on the number of lives left
            if (lives == 2)
            {
                moveSpeed = 6f; // Speed after losing 1 life
            }
            else if (lives == 1)
            {
                moveSpeed = 8f; // Speed after losing 2 lives
            }
            else if (lives == 0)
            {
                moveSpeed = 10f; // Speed after losing all lives
            }
        }
    }

    private IEnumerator RedFlashCoroutine()
    {
        SpriteRenderer spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        AudioSource audioSource = GetComponent<AudioSource>();

        if (spriteRenderer != null && audioSource != null)
        {
            // Play the audio
            audioSource.Play();

            // Change color to red
            spriteRenderer.color = Color.red;

            // Wait for 1.5 seconds
            yield return new WaitForSeconds(0.75f);

            // Change color back to white
            spriteRenderer.color = Color.white;
        }
    }
}
