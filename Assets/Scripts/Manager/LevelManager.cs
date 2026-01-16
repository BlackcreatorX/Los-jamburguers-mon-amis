using UnityEngine;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour
{
    [Header("Settings")]
    public Transform cameraTarget;
    public GameObject roomPrefab;
    public GameObject obstaclePrefab;
    public float roomHeight = 10f;
    [Header("Difficulty")]
    public int levelForDoubleObstacle = 10;
    [Header("Optimization")]
    public int roomsToKeepBelow = 3;

    private Queue<GameObject> activeRooms = new Queue<GameObject>();
    private float nextSpawnY = 0;

    void Start()
    {
        // Generar 3 habitaciones iniciales
        for (int i = 0; i < 5; i++)
        {
            SpawnRoom();
        }
    }

    void SpawnRoom()
    {
        GameObject room = Instantiate(roomPrefab, new Vector3(0, nextSpawnY, 0), Quaternion.identity);
        
        // LOGICA DE GENERACIÓN DE OBSTACULOS DENTRO DEL CUARTO
        // Generamos un obstáculo hijo al azar
        if (nextSpawnY > 0)
        {
            GenerateObstacle(room, "Spawner");

            if (GameManager.Instance.currentLevel >= levelForDoubleObstacle)
            {
                GenerateObstacle(room, "Spawner2");
            }
        }

        nextSpawnY += roomHeight;
        activeRooms.Enqueue(room);

        // Logica para borrar la habitacion mas vieja
        CleanOldRooms();
    }

    void CleanOldRooms()
    {
        if (activeRooms.Count > 0)
        {
            GameObject oldestRoom = activeRooms.Peek();

            float deleteThresholdY = cameraTarget.position.y - (roomsToKeepBelow * roomHeight);

            if (oldestRoom.transform.position.y < deleteThresholdY)
            {
                GameObject roomToDelete = activeRooms.Dequeue();
                Destroy(roomToDelete);
                CleanOldRooms();
            }
        }
    }

    void GenerateObstacle(GameObject room, string spawnerName)
    {
        Transform spawner = room.transform.Find(spawnerName);

        if (spawner != null)
        {
            GameObject obs = Instantiate(obstaclePrefab, spawner.position, Quaternion.identity);
            obs.transform.parent = room.transform;

            Obstacle obsScript = obs.GetComponent<Obstacle>();

            // Asignar el tipo de forma aleatoria
            int randomType = Random.Range(0, 3);
            obsScript.type = randomType;

            // Debug de consola
            string typeName = "";
            if (randomType == 0) typeName = "Estático";
            if (randomType == 1) typeName = "Móvil";
            if (randomType == 2) typeName = "Resistente";
            Debug.Log("Generado obstáculo " + typeName + " en habitación Y=" + room.transform.position.y);
        } else
        {
            if (spawnerName == "Spawner2")
            Debug.LogWarning("No se encontró el Spawner2 en la habitación para generar el segundo obstáculo.");
        }
    }

    public void MoveToNextRoom()
    {
        // Mover cámara arriba
        cameraTarget.position += new Vector3(0, roomHeight, 0);
        
        // Reciclar habitaciones: Destruir la de abajo y crear una nueva arriba
        SpawnRoom();
    }

    public void MoveCameraBackwards(int amount)
    {
        // Mover el target hacia abajo
        cameraTarget.position -= new Vector3(0, roomHeight * amount, 0);
    }
}