using UnityEngine;
using System.Collections.Generic;

public class RoomGenerator : MonoBehaviour
{
    public GameObject roomPrefab; // El prefab base de la habitación
    public int poolSize = 3;
    public float roomHeight = 10f; // Altura en unidades de Unity de cada cuarto

    private Queue<Transform> roomPool;
    private int spawnedRoomsCount = 0;

    void Start()
    {
        roomPool = new Queue<Transform>();

        // Generar las primeras 3 habitaciones
        for (int i = 0; i < poolSize; i++)
        {
            SpawnRoom(i * roomHeight);
        }
    }

    void SpawnRoom(float yPosition)
    {
        GameObject room = Instantiate(roomPrefab, new Vector3(0, yPosition, 0), Quaternion.identity);
        roomPool.Enqueue(room.transform);
        spawnedRoomsCount++;
        
        // Aquí llamarías a una función dentro del script de la Room 
        // para que genere obstáculos aleatorios según la dificultad
        // room.GetComponent<Room>().GenerateObstacles(GameManager.Instance.currentRoomIndex);
    }

    // Llama a esto cuando el jugador llegue a la "meta" de una habitación
    public void CycleRoom()
    {
        Transform oldRoom = roomPool.Dequeue();
        
        // Calcular nueva posición (arriba de la última)
        // La posición es: (habitaciones totales generadas * altura)
        float newY = spawnedRoomsCount * roomHeight;
        
        oldRoom.position = new Vector3(0, newY, 0);
        
        // Resetear y generar nuevos obstáculos en esa habitación reciclada
        // oldRoom.GetComponent<Room>().ResetAndGenerate(...);
        
        roomPool.Enqueue(oldRoom);
        spawnedRoomsCount++;
    }
}