using UnityEngine;

public class DoorMono : MonoBehaviour
{
    [SerializeField] private Vector3 m_gizmoSize;
    [SerializeField] private int m_leversToUnlock = 1;
    [SerializeField] private Vector3 m_openedDoorPosition;
    [SerializeField] private float m_timeToOpenOrClose = 1.0f;

    private Vector3 m_closedPosition;
    private int m_leversInUse = 0;
    private float m_timeMoving = 0.0f;

    private void Awake()
    {
        m_closedPosition = transform.position;
        m_timeMoving = 0.0f;
    }

    public void ToggleLever(bool added)
    {
        m_leversInUse += added ? 1 : -1;
    }

    private void Update()
    {
        if(m_leversInUse >= m_leversToUnlock && transform.position != m_openedDoorPosition)
        {
            m_timeMoving += Time.deltaTime;
            transform.position = Vector3.Lerp(m_closedPosition, m_openedDoorPosition, Mathf.Min(1.0f, m_timeMoving / m_timeToOpenOrClose));
        }
        else if (m_leversInUse < m_leversToUnlock && transform.position != m_closedPosition)
        {
            m_timeMoving += Time.deltaTime;
            transform.position = Vector3.Lerp(m_openedDoorPosition, m_closedPosition, Mathf.Min(1.0f, m_timeMoving / m_timeToOpenOrClose));
        }

        if(m_timeMoving > m_timeToOpenOrClose)
        {
            m_timeMoving = 0.0f;
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(Application.isPlaying ? m_closedPosition : transform.position, m_gizmoSize);        

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(m_openedDoorPosition, m_gizmoSize);
    }
#endif
}
