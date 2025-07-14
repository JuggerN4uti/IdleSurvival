using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Island : MonoBehaviour
{
    [Header("Scripts")]
    public Player PlayerScript;

    [Header("Spawn")]
    public Transform SpawnPoint;
    public Transform DistancePoint, DistanceRotation;
    public Vector3 IslandCenter;
    public GameObject[] SpawnedMob, SpawnedResource;
    public GameObject[] MobPrefab, ResourcePrefab;
    private Mobile MobileSpawned;
    private Resource ResourceSpawned;
    bool spawned;
    int roll, place;

    void Start()
    {
        //IslandCenter = SpawnPoint.position;
        SpawnAll();
        Invoke("RespawnMob", 8f);
        Invoke("RespawnStone", 12f);
    }

    void SpawnAll()
    {
        for (int i = 0; i < SpawnedMob.Length; i++)
        {
            SpawnMob(i);
        }
        for (int i = 0; i < SpawnedResource.Length; i++)
        {
            SpawnStone(i);
        }
    }

    void SpawnMob(int position)
    {
        DistancePoint.position = new Vector2(DistanceRotation.position.x, Random.Range(0.25f, 2.75f) + DistanceRotation.position.y);
        DistanceRotation.rotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));
        roll = Random.Range(0, MobPrefab.Length);

        GameObject mob = Instantiate(MobPrefab[roll], DistancePoint.position, transform.rotation);
        SpawnedMob[position] = mob;
        MobileSpawned = mob.GetComponent(typeof(Mobile)) as Mobile;
        MobileSpawned.PlayerScript = PlayerScript;
        MobileSpawned.SetExpire(Random.Range(70f, 90f));
    }

    void SpawnStone(int position)
    {
        DistancePoint.position = new Vector2(DistanceRotation.position.x, Random.Range(0.3f, 3f) + DistanceRotation.position.y);
        DistanceRotation.rotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));
        roll = Random.Range(0, ResourcePrefab.Length);

        GameObject stone = Instantiate(ResourcePrefab[roll], DistancePoint.position, transform.rotation);
        SpawnedResource[position] = stone;
        ResourceSpawned = stone.GetComponent(typeof(Resource)) as Resource;
        ResourceSpawned.SetExpire(Random.Range(70f, 90f));
    }

    void RespawnMob()
    {
        spawned = false;
        place = 0;
        do
        {
            if (!SpawnedMob[place])
            {
                spawned = true;
                SpawnMob(place);
            }
            else place++;
        } while (!spawned && place < SpawnedMob.Length);
        Invoke("RespawnMob", 8f);
    }

    void RespawnStone()
    {
        spawned = false;
        place = 0;
        do
        {
            if (!SpawnedResource[place])
            {
                spawned = true;
                SpawnStone(place);
            }
            else place++;
        } while (!spawned && place < SpawnedResource.Length);
        Invoke("RespawnStone", 12f);
    }
}
