using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class SlowTrap : Trap
{
    [Range(0.001f, 1.0f)] public float m_speedPercentageInTrap = 0.5f;
}
