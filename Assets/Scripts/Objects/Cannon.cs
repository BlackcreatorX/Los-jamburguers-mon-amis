using UnityEngine;

public class Cannon : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float shootForce = 15f;
    
    private bool canShoot = true;
    private float timeVal = 0;

    void Update()
    {
        // 1. Oscilación (-45 a 45 grados)
        // Obtenemos velocidad según dificultad
        float speed = GameManager.Instance.GetDifficultySpeed();
        timeVal += Time.deltaTime * speed;
        
        // PingPong genera valor entre 0 y 1. Lerp lo convierte a ángulos.
        float angle = Mathf.Lerp(-45f, 45f, Mathf.PingPong(timeVal, 1f));
        transform.rotation = Quaternion.Euler(0, 0, angle);

        // 2. Disparo (Click izquierdo)
        if (Input.GetMouseButtonDown(0) && canShoot)
        {
            Shoot();
        }
    }

    void Shoot()
    {
        canShoot = false;
        GameObject p = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        // Destruir bala después de 3 seg si no choca con nada para limpiar memoria
        Destroy(p, 3f); 
        p.GetComponent<Rigidbody2D>().AddForce(firePoint.up * shootForce, ForceMode2D.Impulse);
    }

    public void ResetCannon()
    {
        canShoot = true;
    }
}