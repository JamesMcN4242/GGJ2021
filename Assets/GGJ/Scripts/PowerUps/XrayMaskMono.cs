using UnityEngine;

public class XrayMaskMono : MonoBehaviour
{
    private enum State
    {
        APPEARING, PRESENT, VANISHING
    }

    [SerializeField] private readonly float k_timeToIncreaseInSize = 0.7f;
    [SerializeField] private readonly float k_timeToVanish = 0.3f;

    [SerializeField] private float k_cameraOffset = 1.2f;

    private float m_timeTracker = 0.0f;
    private State m_currentState = State.APPEARING;

    private Transform m_trackingPlayerTransform;
    private Transform m_cameraTransform;
    private Vector3 m_positionOffset;

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(m_cameraTransform.position, m_trackingPlayerTransform.position);
    }

    // Update is called once per frame
    void Update()
    {
        if(m_trackingPlayerTransform == null || m_cameraTransform == null)
        {
            DestroyImmediate(gameObject);
        }

        Vector3 direction = (m_trackingPlayerTransform.position + m_positionOffset - m_cameraTransform.position).normalized;
        transform.position = m_cameraTransform.position + direction * k_cameraOffset;

        switch(m_currentState)
        {
            case State.APPEARING:
            {
                m_timeTracker += Time.deltaTime;
                if(m_timeTracker >= k_timeToIncreaseInSize)
                {
                    //TODO: Define size better
                    transform.localScale = Vector3.one;
                    m_currentState = State.PRESENT;
                    break;
                }

                transform.localScale = Vector3.one * (m_timeTracker / k_timeToIncreaseInSize);
                break;
            }

            case State.VANISHING:
            {
                m_timeTracker += Time.deltaTime;
                if (m_timeTracker >= k_timeToVanish)
                {
                    Destroy(this.gameObject);
                    break;
                }

                transform.localScale = Vector3.one * (1.0f - (m_timeTracker / k_timeToVanish));
                break;
            }

            case State.PRESENT:
            default:
                break;
        }
    }

    public void SetTargets(Transform playerTransform, Transform cameraTransform, Vector3 targetOffset)
    {
        m_cameraTransform = cameraTransform;
        m_trackingPlayerTransform = playerTransform;
        m_positionOffset = targetOffset;
    }

    public void SetToDestroy()
    {
        m_currentState = State.VANISHING;
        m_timeTracker = 0.0f;
    }
}
