using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [Header("Level Info")]
    public int currentLevel = 0;
    public int totalLevels = 30;
    public float roomHeight = 10f;

    [Header("Oxygen Info")]
    public float maxOxygen = 100f;
    public float currentOxygen;
    public float oxygenCostPerMove = 0.5f;

    // Referencia al LevelManager para pedirle que mueva la cámara
    public LevelManager levelManager;
    public Cannon currentCannon;

    void Awake() { if (Instance == null) Instance = this; }

    void Start()
    {
        currentOxygen = maxOxygen;
    }

    public void AddLevel()
    {
        currentLevel++;
        Debug.Log("Nivel: " + currentLevel);
        oxygenCostPerMove += 0.05f; // Aumenta el costo de oxígeno por movimiento
        if(currentLevel >= totalLevels) Debug.Log("¡GANASTE LLEGASTE A LA LUNA!");
    }

    public void ConsumeOxygen()
    {
        currentOxygen -= oxygenCostPerMove;
        if (currentOxygen <= 0)
        {
            currentOxygen = 0;
            GameOver();
        }

        Debug.Log("Oxígeno restante: " + currentOxygen);
    }

    public void RestoreOxygen(float amount)
    {
        currentOxygen += amount;
        if (currentOxygen > maxOxygen) currentOxygen = maxOxygen;
        Debug.Log("Oxígeno restaurado: " + currentOxygen);
    }

    public void Penalize(int roomsBack)
    {
        AudioManager.Instance.PlayImpact();
        int targetLevel = currentLevel - roomsBack;
        int lowestSaveLevel = levelManager.oldestAliveLevel;

        if (targetLevel < lowestSaveLevel)
        {
            GameOver();
            return;
        }

        int difference = currentLevel - targetLevel;
        currentLevel = targetLevel;

        levelManager.MoveCameraBackwards(difference);
        if (currentCannon != null) currentCannon.ResetCannon();
    }
    
    // Calcula la dificultad (velocidad) basada en el nivel
    public float GetDifficultySpeed()
    {
        return 1.5f + (currentLevel * 0.2f);
    }

    void GameOver()
    {
        Debug.Log("Game Over! Reiniciando el juego...");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}