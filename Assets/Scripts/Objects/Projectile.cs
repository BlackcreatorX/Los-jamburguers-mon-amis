using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody2D rb;

    private float launchCooldown = 0.2f;
    private float timeAlive = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        timeAlive += Time.deltaTime;

        float moveInput = 0;
        if (Input.GetKey(KeyCode.A)) moveInput = -1;
        if (Input.GetKey(KeyCode.D)) moveInput = 1;

        if (moveInput != 0)
        {
            Vector2 force = new Vector2(moveInput * moveSpeed * Time.deltaTime, 0);
            rb.AddForce(force, ForceMode2D.Impulse);

            GameManager.Instance.ConsumeOxygen();

            transform.Rotate(0, 0, -moveInput * 2f);
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Cannon"))
        {
            Cannon cannon = other.GetComponent<Cannon>();
            if (cannon != null && !cannon.isPlayerInside)
            {
                cannon.CatchPlayer(this.gameObject);
            }
        }

        if (other.CompareTag("Oxygen"))
        {
            Destroy(other.gameObject);
            GameManager.Instance.RestoreOxygen(25f);
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (timeAlive < launchCooldown) return;
        
        // Si chocamos con un obstáculo
        if (other.gameObject.CompareTag("Obstacle"))
        {
            Obstacle obs = other.gameObject.GetComponent<Obstacle>();
            Debug.Log("Bala chocó con obstáculo de tipo " + obs.type);
            if(obs != null) obs.HitPlayer();
            
            Destroy(gameObject); // Destruir bala
        }
        else 
        {
            // Si chocas con la pared normal, simplemente se destruye la bala y pierdes el turno
            FindFirstObjectByType<Cannon>().ResetCannon();
            Destroy(gameObject);
        }
    }
}