using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance;

    SpawnPoint[] SpawnPoints;

    void Awake()
    {
        Instance = this;
        SpawnPoints = GetComponentsInChildren<SpawnPoint>();
    }

    public Transform GetSpawnpoint()
    {
        return SpawnPoints[Random.Range(0, SpawnPoints.Length)].transform;
    }
}
