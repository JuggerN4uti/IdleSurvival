using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Spawn")]
    public GameObject SpawnPrefab;
    public GameObject[] SpawnedObject;
    public Vector3[] SpawnPoints;
    public bool[] placeTaken;
    private Resource ResourceSpawned;

    [Header("Stats")]
    public float spawnCheckFrequency;
    bool spawned;
    int place, roll;

    void Start()
    {
        SpawnAll();
        Invoke("Respawn", spawnCheckFrequency);
    }

    void SpawnAll()
    {
        for (int i = 0; i < SpawnedObject.Length; i++)
        {
            Spawn(i);
        }
    }

    void Spawn(int position)
    {
        do
        {
            roll = Random.Range(0, SpawnPoints.Length);
        } while (placeTaken[roll]);

        GameObject spawnedObject = Instantiate(SpawnPrefab, SpawnPoints[roll], transform.rotation);
        SpawnedObject[position] = spawnedObject;
        placeTaken[roll] = true;
        ResourceSpawned = spawnedObject.GetComponent(typeof(Resource)) as Resource;
        ResourceSpawned.SpawnerScript = this;
        ResourceSpawned.spawnerID = roll;
    }

    void Respawn()
    {
        spawned = false;
        place = 0;
        do
        {
            if (!SpawnedObject[place])
            {
                spawned = true;
                Spawn(place);
            }
            else place++;
        } while (!spawned && place < SpawnedObject.Length);

        Invoke("Respawn", spawnCheckFrequency);
    }
}
