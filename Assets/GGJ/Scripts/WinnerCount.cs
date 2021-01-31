using UnityEngine;

public class WinnerCount : MonoBehaviour
{
    public static int s_winnerCount = 0;
    
    private void OnTriggerEnter(Collider other)
    {
        var player = other.GetComponent<PositionMono>();
        if (player != null)
        {
            player.m_escaped = false;
            ++s_winnerCount; // this is terrible and if people manage to make it to the end of the area and back it will break it    
        }
        
    }
}
