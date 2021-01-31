using FMODUnity;
using UnityEngine;

public class PlayerAudioMovement : MonoBehaviour
{
    [EventRef] public string m_inputWalkSound;
    [EventRef] public string m_inputRunSound;
    public float m_secondsBetweenSFX = 2.0f;

    public float m_walkingSpeed;
    private float m_timePassed = 0.0f;

    private bool IsWalking => m_walkingSpeed > 0.05f;


    private void Update()
    {
        if(IsWalking)
        {
            m_timePassed += Time.deltaTime;
            if (m_timePassed >= m_secondsBetweenSFX)
            {
                string inputSound = m_walkingSpeed > 5.0f ? m_inputRunSound : m_inputWalkSound;
                RuntimeManager.PlayOneShotAttached(inputSound, gameObject);
                m_timePassed = 0.0f;
            }
        }
        else
        {
            m_timePassed = 0.0f;
        }
    }
}
