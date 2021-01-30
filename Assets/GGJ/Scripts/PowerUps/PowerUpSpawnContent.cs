using System;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/PowerUpSpawnContent", fileName = "PowerUpSpawnContent.asset")]
public class PowerUpSpawnContent : ScriptableObject
{
    public PowerUpContent[] m_powerUpsToSpawn;

    private void OnValidate()
    {
        for(int i = 0; i < m_powerUpsToSpawn.Length; i++)
        {
            if(m_powerUpsToSpawn[i].m_powerUpPrefabPath.StartsWith("PowerUpPrefabs/") == false)
            {
                m_powerUpsToSpawn[i].m_powerUpPrefabPath = "PowerUpPrefabs/" + m_powerUpsToSpawn[i].m_powerUpPrefabPath;
            }
        }
    }
}

[Serializable]
public struct PowerUpContent
{
    public PowerUpTypes m_powerUpType;
    public string m_powerUpPrefabPath;
    public int m_numberToSpawn;
}