using UnityEngine;

public class Obstacle : MonoBehaviour
{
    // 0 = Estático (regresa 1), 1 = Móvil (regresa 1), 2 = Resistente (1 golpe reset, 2 golpes regresa 2)
    public int type = 0; 
    
    // Variables para obstáculo móvil
    private float startX;
    private float speed = 2f;

    // Variables para obstáculo resistente
    private int hitsTaken = 0; 
    private SpriteRenderer sr;

    void Start()
    {
        startX = transform.position.x;
        sr = GetComponent<SpriteRenderer>();
        
        // Ajustar color según tipo para diferenciarlos visualmente
        if(type == 0) sr.color = Color.gray; // Estático
        if(type == 1) sr.color = Color.yellow; // Móvil
        if(type == 2) sr.color = Color.red; // Resistente
    }

    void Update()
    {
        // Lógica de movimiento (Solo tipo 1)
        if (type == 1)
        {
            float x = Mathf.PingPong(Time.time * speed, 4f) - 2f; // Mueve entre -2 y 2
            transform.position = new Vector3(startX + x, transform.position.y, 0);
        }
    }

    public void HitPlayer()
    {
        if (type == 0 || type == 1)
        {
            // Regresa 1 habitación
            GameManager.Instance.Penalize(1);
        }
        else if (type == 2)
        {
            hitsTaken++;
            if (hitsTaken == 1)
            {
                // Primer golpe: solo resetea el tiro, no te regresa habitaciones
                sr.color = Color.white; // Feedback visual
                FindFirstObjectByType<Cannon>().ResetCannon();
            }
            else
            {
                // Segundo golpe: te regresa 2 habitaciones
                hitsTaken = 0;
                sr.color = Color.red;
                GameManager.Instance.Penalize(2);
            }
        }
    }
}