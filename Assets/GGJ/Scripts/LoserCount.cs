using System;
using UnityEngine;

public class LoserCount : MonoBehaviour
{
    public static int s_loserCount = 0;
    private void OnTriggerEnter(Collider other)
    {
        var player = other.GetComponent<PositionMono>();
        if (player != null && !player.IsSeeker)
        {
            player.m_escaped = false;
            ++s_loserCount;     
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        var player = other.GetComponent<PositionMono>();
        if (player != null && !player.IsSeeker)
        {
            player.m_escaped = false;
            --s_loserCount;  
        }
    }
}
