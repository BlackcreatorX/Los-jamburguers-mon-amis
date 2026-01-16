using UnityEngine;

public class Projectile : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        // Si tocamos el sensor de "Siguiente Nivel"
        if (other.CompareTag("LevelTrigger"))
        {
            GameManager.Instance.AddLevel();
            // Avisar al LevelManager para generar siguiente cuarto y subir cámara
            FindFirstObjectByType<LevelManager>().MoveToNextRoom();
            
            FindFirstObjectByType<Cannon>().ResetCannon();
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        // Si chocamos con un obstáculo
        if (other.gameObject.CompareTag("Obstacle"))
        {
            Obstacle obs = other.gameObject.GetComponent<Obstacle>();
            if(obs != null) obs.HitPlayer();
            
            Destroy(gameObject); // Destruir bala
        }
        else 
        {
            // Si chocas con la pared normal, simplemente se destruye la bala y pierdes el turno
            // (Opcional: puedes hacer que rebote, pero aquí reseteamos el cañón)
            FindFirstObjectByType<Cannon>().ResetCannon();
            Destroy(gameObject);
        }
    }
}