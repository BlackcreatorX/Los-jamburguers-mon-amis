using UnityEngine;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour
{
    public Transform cameraTransform;
    public GameObject roomPrefab;
    public GameObject obstaclePrefab;
    public float roomHeight = 10f;
    
    // Cola para guardar las habitaciones activas
    private Queue<GameObject> activeRooms = new Queue<GameObject>();
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
        
        // LOGICA DE GENERACIÓN DE OBSTACULOS DENTRO DEL CUARTO
        // Generamos un obstáculo hijo al azar
        // Nota: Asegurate de tener un objeto vacio llamado "Spawner" en tu prefab Room
        Transform spawner = room.transform.Find("Spawner");
        if(spawner != null && nextSpawnY > 0) // No poner obstaculos en el primer cuarto (nivel 0)
        {
        GameObject obs = Instantiate(obstaclePrefab, spawner.position, Quaternion.identity);
        obs.transform.parent = room.transform;
        
        Obstacle obsScript = obs.GetComponent<Obstacle>();
        obsScript.type = Random.Range(0, 3);
        }

        nextSpawnY += roomHeight;
        activeRooms.Enqueue(room);
    }

    public void MoveToNextRoom()
    {
        // Mover cámara arriba
        cameraTransform.position += new Vector3(0, roomHeight, 0);
        
        // Reciclar habitaciones: Destruir la de abajo y crear una nueva arriba
        // (En un juego final usaríamos Pooling real sin Destroy, pero para prototipo esto sirve)
        GameObject oldRoom = activeRooms.Dequeue();
        Destroy(oldRoom);
        SpawnRoom();
    }

    public void MoveCameraBackwards(int amount)
    {
        // Mover cámara abajo
        cameraTransform.position -= new Vector3(0, roomHeight * amount, 0);
        
        // Nota técnica: Al regresar, las habitaciones de arriba deberían destruirse
        // y regenerarse las de abajo. Para este prototipo rápido,
        // simplemente moveremos la cámara. El fondo visual se verá raro
        // pero la mecánica funcionará.
    }
}