using System;

[Serializable]
public enum PowerUpTypes
{
    NONE, SPEED_BOOST, X_RAY
}

public struct PowerUpData
{
    public PowerUpTypes m_type;
    public float m_secondsRemaining;
    public float m_affectingValue;
}