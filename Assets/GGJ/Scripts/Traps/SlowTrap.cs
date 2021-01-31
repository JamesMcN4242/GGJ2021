using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class SlowTrap : Trap
{
    [Range(0.001f, 1.0f)] public float m_speedPercentageInTrap = 0.5f;
    private BoxCollider m_trigger = null;

    private void Awake()
    {
        m_trigger = GetComponent<BoxCollider>();
    }

    public void Toggle(bool active)
    {
        m_trigger.enabled = active;

        //TODO: Play any appropriate animation here
    }
}
