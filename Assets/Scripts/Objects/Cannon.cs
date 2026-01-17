using System.Collections;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    [Header("Settings")]
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float shootForce = 15f;

    [Header("State")]
    public bool isPlayerInside = false;
    public bool isTargetCannon = false;
    
    private float timeVal = 0;

    private float horizontalSpeed = 2f;
    private float horizontalRange = 2.5f;
    private float startX;

    [SerializeField] ParticleSystem cannonParticles;

    void Start()
    {
        startX = transform.position.x;
        if (isPlayerInside)
        {
            GameManager.Instance.currentCannon = this;
            AudioManager.Instance.Playidle();
        }
    }

    void Update()
    {
        if (isPlayerInside)
        {
            // 1. Oscilación (-45 a 45 grados)
            // Obtenemos velocidad según dificultad
            float speed = GameManager.Instance.GetDifficultySpeed();
            timeVal += Time.deltaTime * speed;
            
            // PingPong genera valor entre 0 y 1. Lerp lo convierte a ángulos.
            float angle = Mathf.Lerp(-45f, 45f, Mathf.PingPong(timeVal, 1f));
            transform.rotation = Quaternion.Euler(0, 0, angle);

            // 2. Disparo (Click izquierdo)
            if (Input.GetMouseButtonDown(0))
            {
                Shoot();
            }
            
        }else if (isTargetCannon)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.identity, Time.deltaTime * 5f);

            float x = Mathf.PingPong(Time.time * horizontalSpeed, horizontalRange * 2) - horizontalRange;
            transform.position = new Vector3(startX + x, transform.position.y, 0);
        }
    }

    void Shoot()
    {
        isPlayerInside = false;
        isTargetCannon = false;

        AudioManager.Instance.StopIdle();
        AudioManager.Instance.PlayShoot();

        GameObject p = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);

        Rigidbody2D rb = p.GetComponent<Rigidbody2D>();
        rb.AddForce(firePoint.up * shootForce, ForceMode2D.Impulse);

        PlayParticles();
    }

    public void CatchPlayer(GameObject penguin)
    {
        Destroy(penguin);

        isPlayerInside = true;
        isTargetCannon = false;

        GameManager.Instance.currentCannon = this;
        AudioManager.Instance.Playidle();

        GameManager.Instance.AddLevel();
        FindFirstObjectByType<LevelManager>().MoveToNextRoom();
    }

    public void ResetCannon()
    {
        isPlayerInside = true;
        AudioManager.Instance.Playidle();
    }    
    public void PlayParticles()
    {
        // Reinicia completamente el sistema (padre + hijos)
        cannonParticles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        cannonParticles.Play(true);
    }
}