using UnityEngine;

public class CannonController : MonoBehaviour
{
    [Header("Configuración")]
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float shootForce = 15f;
    public float maxAngle = 45f;

    private bool canShoot = true;
    private float timeCounter = 0f;

    void Update()
    {
        HandleRotation();
        HandleShooting();
    }

    void HandleRotation()
    {
        if (!canShoot) return; // Si ya disparaste, el cañón se detiene (opcional)

        // Obtenemos la velocidad basada en la dificultad del GameManager
        float speed = GameManager.Instance.GetDifficultySpeed();
        
        // PingPong para oscilar de 0 a 1, luego mapeamos a ángulos
        timeCounter += Time.deltaTime * speed;
        float t = Mathf.PingPong(timeCounter, 1f);
        
        // Lerp entre -45 y 45 grados
        float angle = Mathf.Lerp(-maxAngle, maxAngle, t);
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    void HandleShooting()
    {
        if (Input.GetMouseButtonDown(0) && canShoot)
        {
            Shoot();
        }
    }

    void Shoot()
    {
        canShoot = false;
        GameObject proj = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = proj.GetComponent<Rigidbody2D>();
        rb.AddForce(firePoint.up * shootForce, ForceMode2D.Impulse);
    }

    // Llama a esto cuando el jugador pierda o pase de nivel para resetear el disparo
    public void ResetCannon()
    {
        canShoot = true;
        // Aquí podrías resetear la rotación a 0 si quieres
    }
}