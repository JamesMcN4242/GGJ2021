using System;
using UnityEngine;

public class WinnerCount : MonoBehaviour
{
    public static int s_winnerCount = 0;
    
    private void OnTriggerEnter(Collider other)
    {
        var player = other.GetComponent<PositionMono>();
        if (player != null && !player.IsSeeker)
        {
            player.m_escaped = true;
            ++s_winnerCount; 
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var player = other.GetComponent<PositionMono>();
        if (player != null && !player.IsSeeker)
        {
            player.m_escaped = true;
            --s_winnerCount; 
        }
    }
}
