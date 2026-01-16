using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public int currentLevel = 0;
    public int totalLevels = 30;
    public float roomHeight = 10f; // Importante: Debe coincidir con el tamaño de tu cuarto

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
        int targetLevel = Mathf.Max(0, currentLevel - roomsBack);
        int difference = currentLevel - targetLevel;
        
        currentLevel = targetLevel;
        
        // Movemos la cámara hacia abajo
        levelManager.MoveCameraBackwards(difference);
        
        // Reseteamos el cañón
        cannon.ResetCannon();
    }
    
    // Calcula la dificultad (velocidad) basada en el nivel
    public float GetDifficultySpeed()
    {
        // Base 1.5f + un poquito más por cada nivel
        return 1.5f + (currentLevel * 0.2f);
    }
}