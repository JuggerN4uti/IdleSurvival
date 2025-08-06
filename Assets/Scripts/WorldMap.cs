using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldMap : MonoBehaviour
{
    [Header("Scripts")]
    public Player PlayerScript;

    [Header("UI")]
    public GameObject WorldScreenObject;
    public GameObject[] PlayerIconObject, IslandObject;
    public GameObject CameraObject;
    public Vector3[] IslandSpawnPoint, CameraPoint;

    public void Set(int playerLocation)
    {
        for (int i = 0; i < PlayerIconObject.Length; i++)
        {
            PlayerIconObject[i].SetActive(false);
        }
        PlayerIconObject[playerLocation].SetActive(true);
    }

    public void SelectIsland(int island)
    {
        IslandObject[PlayerScript.island].SetActive(false);
        PlayerScript.ChangeTask();
        PlayerScript.island = island;
        IslandObject[island].SetActive(true);
        PlayerScript.transform.position = IslandSpawnPoint[island];
        CameraObject.transform.position = CameraPoint[island];
        PlayerScript.movePos = IslandSpawnPoint[island];
        WorldScreenObject.SetActive(false);
    }
}
