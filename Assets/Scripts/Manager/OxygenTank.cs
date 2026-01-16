using UnityEngine;
using System.Collections;

public class OxygenTank : MonoBehaviour
{
    [Header("Configuración del tanque")]
    [SerializeField] private float duracionTotal = 30f; // segundos
    private float oxigenoActual;

    private void Start()
    {
        oxigenoActual = duracionTotal;
        StartCoroutine(ConsumirOxigeno());
    }

    IEnumerator ConsumirOxigeno()
    {
        while (oxigenoActual > 0)
        {
            yield return new WaitForSeconds(1f);

            oxigenoActual--;

            float porcentaje = (oxigenoActual / duracionTotal) * 100f;
            Debug.Log($"Oxígeno restante: {porcentaje:F0}%");

            if (oxigenoActual <= 0)
            {
                oxigenoActual = 0;
                Debug.Log("Oxígeno agotado");
                // Aquí puedes disparar muerte, daño, game over, etc.
            }
        }
    }
}
