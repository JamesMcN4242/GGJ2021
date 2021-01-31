using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider))]
public class LeverMono : MonoBehaviour
{
    [Tooltip("Time for the lever to resest. < 0 if this never happens")] [SerializeField] private float m_resetTimer = -1.0f;
    [Tooltip("Whether players can turn the level off by re-enteracting with it")] [SerializeField] private bool m_canBeTurnedOff = false;
    [SerializeField] private UnityEvent<bool> m_clickedAction = null;
    
    private bool m_leverOn = false;
    private float m_timeReseting = 0.0f;

    public void Toggle()
    {
        if (m_leverOn && !m_canBeTurnedOff) return;
        ToggleInternal();
    }

    private void ToggleInternal()
    {
        m_leverOn = !m_leverOn;
        m_clickedAction?.Invoke(m_leverOn);
    }

    private void Update()
    {
        if(m_leverOn && m_resetTimer > 0.0f)
        {
            m_timeReseting += Time.deltaTime;
            if(m_timeReseting > m_resetTimer)
            {
                ToggleInternal();
                m_timeReseting = 0.0f;
            }
        }
    }
}
