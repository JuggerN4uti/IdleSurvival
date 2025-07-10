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
    public GameObject[] SpawnedMob, MobPrefab;
    private Mobile MobileSpawned;
    bool spawned;
    int roll, place;

    void Start()
    {
        //IslandCenter = SpawnPoint.position;
        SpawnAllMobs();
        Invoke("Respawn", 5f);
    }

    void SpawnAllMobs()
    {
        for (int i = 0; i < SpawnedMob.Length; i++)
        {
            SpawnMob(i);
        }
    }

    void SpawnMob(int position)
    {
        DistancePoint.position = new Vector2(DistanceRotation.position.x, Random.Range(0.25f, 2.75f) + DistanceRotation.position.y);
        DistanceRotation.rotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));
        //roll = Random.Range(0, MobPrefab.Length);

        GameObject mob = Instantiate(MobPrefab[position], DistancePoint.position, transform.rotation);
        SpawnedMob[position] = mob;
        MobileSpawned = mob.GetComponent(typeof(Mobile)) as Mobile;
        MobileSpawned.PlayerScript = PlayerScript;
    }

    void Respawn()
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
        Invoke("Respawn", 5f);
    }
}
