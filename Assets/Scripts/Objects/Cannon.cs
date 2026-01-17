using System.Collections;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    [Header("Settings")]
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float shootForce = 15f;
    public float reloadTime = 0.6f;

    private bool canShoot = false;
    private float timeVal = 0;
    private bool isReloading = false;
    [SerializeField] ParticleSystem cannonParticles;

    void Start()
    {
        AudioManager.Instance.Playidle();
        StartCoroutine(EnableShooting(0.5f));
    }

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
        if (Input.GetMouseButtonDown(0) && canShoot && !isReloading)
        {
            Shoot();
        }
       
    }

    void Shoot()
    {
        canShoot = false;

        // Audio
        AudioManager.Instance.StopIdle();
        AudioManager.Instance.PlayShoot();

        GameObject p = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        Destroy(p, 3f);
        p.GetComponent<Rigidbody2D>().AddForce(firePoint.up * shootForce, ForceMode2D.Impulse);
        PlayParticles();
    }

    public void ResetCannon()
    {
        if (!isReloading)
        {
            StartCoroutine(ReloadCoroutine());
        }
    }

    IEnumerator ReloadCoroutine()
    {
        // isReloading = true;

        yield return new WaitForSeconds(reloadTime);

        canShoot = true;
        object isReloading = false;

        AudioManager.Instance.Playidle();
    }

    IEnumerator EnableShooting(float delay)
    {
        yield return new WaitForSeconds(delay);
        canShoot = true;
    }    
    public void PlayParticles()
    {
        // Reinicia completamente el sistema (padre + hijos)
        cannonParticles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        cannonParticles.Play(true);
    }
}