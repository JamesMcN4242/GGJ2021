using Photon.Pun;
using UnityEngine;

[RequireComponent(typeof(PhotonView))]
public abstract class PowerUpMono : MonoBehaviour
{
    [SerializeField] protected float m_timeAffecting = 5.0f;
    [SerializeField] private float m_timeBeforeRespawn = 15.0f;

    private bool m_respawning = false;
    private float m_timeLeftTillRespawn = 0.0f;

    private Collider m_collider;
    private Renderer m_renderer;

    //[SerializeField private float 
    public abstract PowerUpData GetPowerUpData();

    private void Awake()
    {
        m_collider = GetComponent<Collider>();
        m_renderer = GetComponent<Renderer>();
    }

    public void SetClaimed()
    {
        PhotonView view = PhotonView.Get(this);
        view.RPC("RPCSetClaimed", RpcTarget.All);
    }

    [PunRPC]
    public void RPCSetClaimed()
    {
        m_respawning = true;
        m_timeLeftTillRespawn = m_timeBeforeRespawn;

        m_collider.enabled = false;
        m_renderer.enabled = false;
    }

    private void Update()
    {
        if(m_respawning)
        {
            m_timeLeftTillRespawn -= Time.deltaTime;
            if(m_timeLeftTillRespawn <= 0.0f)
            {
                m_respawning = false;

                m_collider.enabled = true;
                m_renderer.enabled = true;
            }
        }
    }
}
