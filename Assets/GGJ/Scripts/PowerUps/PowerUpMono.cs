using UnityEngine;

public abstract class PowerUpMono : MonoBehaviour
{
    [SerializeField] protected float m_timeAffecting = 5.0f;
    public abstract PowerUpData GetPowerUpData();
}
