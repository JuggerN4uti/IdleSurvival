using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Island : MonoBehaviour
{
    public Transform SpawnPoint;
    public Vector3 IslandCenter;

    void Start()
    {
        IslandCenter = SpawnPoint.position;
    }
}
