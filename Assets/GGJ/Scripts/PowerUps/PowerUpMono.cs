using UnityEngine;

public abstract class PowerUpMono : MonoBehaviour
{
    [SerializeField] protected float m_timeAffecting = 5.0f;

    //[SerializeField private float 
    public abstract PowerUpData GetPowerUpData();

    private void Update()
    {
        
    }
}
