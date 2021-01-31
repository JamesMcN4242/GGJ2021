using UnityEngine;

public class SpeedBoostPowerUpMono : PowerUpMono
{
    [SerializeField] private float m_speedMultiplier = 2.0f;

    public override PowerUpData GetPowerUpData()
    {
        return new PowerUpData() { m_type = PowerUpTypes.SPEED_BOOST, m_secondsRemaining = m_timeAffecting, m_affectingValue = m_speedMultiplier, m_secondsTotal = m_timeAffecting };
    }
}
