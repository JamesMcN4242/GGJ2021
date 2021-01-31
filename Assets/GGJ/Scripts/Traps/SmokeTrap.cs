using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class SmokeTrap : Trap
{
    private ParticleSystem m_particles = null;

    private void Awake()
    {
        m_particles = GetComponent<ParticleSystem>();
    }

    public void Toggle(bool active)
    {
        if(!active)
        {
            m_particles.Stop();
        }
        else
        {
            m_particles.Play();
        }
    }
}
