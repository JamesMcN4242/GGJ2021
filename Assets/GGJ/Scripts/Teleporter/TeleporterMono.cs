using UnityEngine;

public enum TeleportType
{
    ENTRY,
    EXIT,
    BOTH
}

public class TeleporterMono : MonoBehaviour
{
    [Range(0,TeleportManager.k_channelCount-1)] public uint m_id;
    public TeleportType m_type = TeleportType.BOTH;
    private TeleportManager m_manager;
    
    public void RegisterManager(TeleportManager manager) { m_manager = manager; }
    
    private void OnTriggerEnter(Collider other)
    {
        var mono = other.GetComponent<PositionMono>();
        if (mono != null && m_type != TeleportType.EXIT)
        {
            m_manager.TeleportPositionMono(this,mono);
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        var mono = other.GetComponent<PositionMono>();
        if (mono != null)
        {
            m_manager.ExitTeleporter(mono);
        }
    }
}
