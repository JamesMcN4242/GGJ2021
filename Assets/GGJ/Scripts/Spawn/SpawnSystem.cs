using System;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using UnityEngine;

using Random = UnityEngine.Random;

public static class SpawnSystem
{ 
    public static void SpawnPowerUps()
    {
        PowerUpContent[] powerUpContents = Resources.Load<PowerUpSpawnContent>("SpawnData/PowerUpSpawnContent").m_powerUpsToSpawn;
        List<Transform> spawnPoints = new List<Transform>(Array.ConvertAll(GameObject.FindObjectsOfType<PowerUpSpawnerMono>(), entry => entry.transform));
        Debug.Assert(spawnPoints.Count >= powerUpContents.Sum(entry => entry.m_numberToSpawn), "More power ups to create than spawn points available");

        foreach(PowerUpContent content in powerUpContents)
        {
            for(int i = 0; i < content.m_numberToSpawn; i++)
            {
                int randomPos = Random.Range(0, spawnPoints.Count);
                PhotonNetwork.Instantiate(content.m_powerUpPrefabPath, spawnPoints[randomPos].position, Quaternion.identity);
                spawnPoints.RemoveAt(randomPos);
            }
        }
    }
}
