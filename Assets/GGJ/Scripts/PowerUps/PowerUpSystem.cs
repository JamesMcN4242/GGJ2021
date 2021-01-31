using UnityEngine;

public static class PowerUpSystem
{
    public static PowerUpData UpdatePowerUpData (PowerUpData powerUpData, Transform localPlayerTrans, float deltaTime)
    {
        switch(powerUpData.m_type)
        {
            case PowerUpTypes.SPEED_BOOST:
            case PowerUpTypes.X_RAY:
                powerUpData.m_secondsRemaining -= deltaTime;
                if (powerUpData.m_secondsRemaining <= 0.0f)
                {
                    TransitionPowerUps(powerUpData, default, localPlayerTrans);
                    return default;
                }
                return powerUpData;

            case PowerUpTypes.NONE:
            default:
                return powerUpData;
        }
    }

    public static (bool hitPowerUp, PowerUpData powerUpData) GetIntersectingPowerUp(Transform player, Vector3 playerSize)
    {
        //We made him super tall since we were running into issues, I regret nothing.
        Collider[] colliders = Physics.OverlapBox(player.position, playerSize.CopyWithY(200.0f), player.rotation, int.MaxValue, QueryTriggerInteraction.Collide);
        foreach (Collider col in colliders)
        {
            PowerUpMono powerUp = col.GetComponent<PowerUpMono>();

            if(powerUp != null)
            {
                powerUp.SetClaimed();
                return (true, powerUp.GetPowerUpData());
            }
        }

        return (false, default);
    }

    public static void TransitionPowerUps(PowerUpData currentData, PowerUpData newData, Transform localPlayerTrans)
    {
        if (currentData.m_type == newData.m_type) return;

        if(currentData.m_type == PowerUpTypes.X_RAY)
        {
            var xrays = Object.FindObjectsOfType<XrayMaskMono>();
            foreach(var xray in xrays)
            {
                xray.SetToDestroy();
            }
        }
        else if (newData.m_type == PowerUpTypes.X_RAY)
        {
            GameObject xrayPrefab = Resources.Load<GameObject>("PowerUpPrefabs/XrayWindow");
            var players = Object.FindObjectsOfType<PositionMono>();
            var cameraTrans = Camera.main.transform;

            foreach(PositionMono pos in players)
            {
                if (pos.transform == localPlayerTrans) continue;

                GameObject obj = GameObject.Instantiate(xrayPrefab);
                obj.GetComponent<XrayMaskMono>().SetTargets(pos.transform, cameraTrans, Vector3.zero.CopyWithY(1.0f));
            }
        }
    }
}
