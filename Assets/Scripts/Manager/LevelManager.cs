using UnityEngine;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour
{
    [Header("Settings")]
    public Transform cameraTarget;
    public GameObject roomPrefab;
    public GameObject obstaclePrefab;
    public GameObject cannonPrefab;
    public GameObject oxygenTankPrefab;
    public float roomHeight = 10f;
    [Header("Difficulty")]
    public int levelForDoubleObstacle = 10;
    [Header("Optimization")]
    public int roomsToKeepBelow = 3;

    public int oldestAliveLevel = 0;

    private Queue<GameObject> activeRooms = new Queue<GameObject>();
    private List<GameObject> roomsWithCows = new List<GameObject>();
    private float nextSpawnY = 0;

    void Start()
    {
        // Generar 3 habitaciones iniciales
        for (int i = 0; i < 3; i++)
        {
            SpawnRoom();
        }
    }

    void SpawnRoom()
    {
        GameObject room = Instantiate(roomPrefab, new Vector3(0, nextSpawnY, 0), Quaternion.identity);

        bool spawnedCow = false;
        
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

        if (nextSpawnY > 0)
        {
            Vector3 cannonPos = new Vector3(0, -3.5f, 0);
            GameObject newCannon = Instantiate(cannonPrefab, room.transform);
            newCannon.transform.localPosition = cannonPos;

            Cannon cScript = newCannon.GetComponent<Cannon>();
            cScript.isPlayerInside = false;
            cScript.isTargetCannon = true;
            newCannon.tag = "Cannon";
        }

        int levelPrediction = GameManager.Instance.currentLevel + activeRooms.Count;
        if (levelPrediction > 0 && levelPrediction % 5 == 0)
        {
            GameObject oxy = Instantiate(oxygenTankPrefab, room.transform);
            oxy.transform.localPosition = new Vector3(Random.Range(-2f, 2f), 2f, 0);
            oxy.tag = "Oxygen";
        }

        if (spawnedCow)
        {
            AudioManager.Instance.PlayCowBell();
            roomsWithCows.Add(room);
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

                oldestAliveLevel++;
                
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
            obs.GetComponent<Obstacle>().type = Random.Range(0, 3);

            // Obstacle obsScript = obs.GetComponent<Obstacle>();

            // Asignar el tipo de forma aleatoria
            // int randomType = Random.Range(0, 3);
            // obsScript.type = randomType;

            // // Debug de consola
            // string typeName = "";
            // if (randomType == 0) typeName = "Estático";
            // if (randomType == 1) typeName = "Móvil";
            // if (randomType == 2) typeName = "Resistente";
            // Debug.Log("Generado obstáculo " + typeName + " en habitación Y=" + room.transform.position.y);
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
        CleanOldRooms();
    }

    void CheckForCowMoo()
    {
        for (int i = roomsWithCows.Count - 1; i >= 0; i--)
        {
            GameObject room = roomsWithCows[i];
            if (room == null)
            {
                roomsWithCows.RemoveAt(i);
                continue;
            }

            if (Mathf.Abs(cameraTarget.position.y - room.transform.position.y) < 1f)
            {
                AudioManager.Instance.PlayCowMoo();
                roomsWithCows.RemoveAt(i);
                return;
            }
        }
    }

    public void MoveCameraBackwards(int amount)
    {
        // Mover el target hacia abajo
        cameraTarget.position -= new Vector3(0, roomHeight * amount, 0);
    }
}