using System.Collections.Generic;
using UnityEngine;

public class TeleportManager
{
    public const uint k_channelCount = 10;
    public List<TeleporterMono>[] m_buckets = new List<TeleporterMono>[k_channelCount];
    public PositionMono m_player;
    public bool m_active = true;
    public int exitCount = 0;

    public void Initialise(PositionMono player)
    {
        m_player = player;
        for (var i = 0; i < m_buckets.Length; i++)
        {
            m_buckets[i] = new List<TeleporterMono>();
        }

        var allTeleporters = GameObject.FindObjectsOfType<TeleporterMono>();
        foreach (var teleporter in allTeleporters)
        { 
            teleporter.RegisterManager(this);
            m_buckets[teleporter.m_id].Add(teleporter); 
        }
    }
    
    public void TeleportPositionMono(TeleporterMono teleporter, PositionMono mono)
    {
        if (mono == m_player && m_active)
        {
            m_active = false;
            var channel = m_buckets[teleporter.m_id];
            if (channel.Count > 1)
            {
                List<TeleporterMono> cp = new List<TeleporterMono>(channel);
                cp.Remove(teleporter);
                for (int i = cp.Count - 1; i >= 0; --i) // Remove all telelporters that aren't exits
                {
                    if(cp[i].m_type == TeleportType.ENTRY)
                        cp.RemoveAt(i);
                }
                int index = Random.Range(0, cp.Count);
                mono.transform.position = cp[index].transform.position;
            }
        }
    }

    public void ExitTeleporter(PositionMono mono)
    {
        if (mono == m_player && !m_active)
        {
            if (++exitCount == 2)
            {
                exitCount = 0;
                m_active = true;
            }
        }
    }
}
