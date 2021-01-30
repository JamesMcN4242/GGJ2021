using UnityEngine;

public class XRayPowerUpMono : PowerUpMono
{
    public override PowerUpData GetPowerUpData()
    {
        return new PowerUpData() { m_type = PowerUpTypes.X_RAY, m_secondsRemaining = m_timeAffecting, m_affectingValue = -1.0f };
    }
}
