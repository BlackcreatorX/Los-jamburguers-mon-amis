using UnityEngine;

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
        int targetLevel = Mathf.Max(0, currentLevel - roomsBack);
        int difference = currentLevel - targetLevel;

        if (difference > 0)
        {
            Debug.Log($"Golpe recibido! Regresando {difference} niveles.");
            currentLevel = targetLevel;
            levelManager.MoveCameraBackwards(difference);
            cannon.ResetCannon();
        }else
        {
            Debug.Log("No se puede penalizar más, ya estás en el nivel 0.");
            cannon.ResetCannon();
        }
    }
    
    // Calcula la dificultad (velocidad) basada en el nivel
    public float GetDifficultySpeed()
    {
        return 1.5f + (currentLevel * 0.2f);
    }
}