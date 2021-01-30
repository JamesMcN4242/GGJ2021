using UnityEngine;

public static class PowerUpSystem
{
    public static PowerUpData UpdatePowerUpData (PowerUpData powerUpData, float deltaTime)
    {
        switch(powerUpData.m_type)
        {
            case PowerUpTypes.SPEED_BOOST:
            case PowerUpTypes.X_RAY:
                powerUpData.m_secondsRemaining -= deltaTime;
                if(powerUpData.m_secondsRemaining <= 0.0f)
                    return default;
                return powerUpData;

            case PowerUpTypes.NONE:
            default:
                return powerUpData;
        }
    }

    public static (bool hitPowerUp, PowerUpData powerUpData) GetIntersectingPowerUp(Transform player, Vector3 playerSize)
    {
        Collider[] colliders = Physics.OverlapBox(player.position, playerSize * 0.5f, player.rotation, int.MaxValue, QueryTriggerInteraction.Collide);
        foreach (Collider col in colliders)
        {
            PowerUpMono powerUp = col.GetComponent<PowerUpMono>();
            if(powerUp != null)
            {
                return (true, powerUp.GetPowerUpData());
            }
        }

        return (false, default);
    }
}
