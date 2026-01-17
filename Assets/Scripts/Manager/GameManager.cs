using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public int currentLevel = 0;
    public int totalLevels = 30;
    public float roomHeight = 10f;

    // Referencia al LevelManager para pedirle que mueva la cámara
    public LevelManager levelManager;
    public Cannon cannon;

    void Awake() { if (Instance == null) Instance = this; }

    public void AddLevel()
    {
        currentLevel++;
        Debug.Log("Nivel: " + currentLevel);
        if(currentLevel >= totalLevels) Debug.Log("¡GANASTE LLEGASTE A LA LUNA!");
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
        cannon.ResetCannon();
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