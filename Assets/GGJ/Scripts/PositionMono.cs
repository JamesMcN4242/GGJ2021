using System;
using Photon.Pun;
using UnityEngine;

public class PositionMono : MonoBehaviour, IPunObservable
{
    private Animator m_animator;
    public Vector3 m_velocity;
    public Vector3 m_start;
    public Vector3 m_end;
    public float m_currentTime = 0;
    public float m_lag;

    public void Awake()
    {
        m_animator = GetComponent<Animator>();
    }

    public void LateUpdate()
    {
        if(Mathf.Abs(m_lag) > 0)
            transform.position = Vector3.Lerp(m_start, m_end, m_currentTime / m_lag);
        m_currentTime += Time.deltaTime;

        m_animator.SetFloat("MovementSpeed", m_velocity.magnitude);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(m_velocity);
            stream.SendNext(transform.rotation);
        }
        else
        {
            Vector3 position = (Vector3)stream.ReceiveNext();
            m_velocity = (Vector3) stream.ReceiveNext();
            transform.rotation = (Quaternion)stream.ReceiveNext();

            m_start = transform.position;
            m_lag = Mathf.Abs((float) (PhotonNetwork.Time - info.SentServerTime)) * 2f;
            m_end = position + m_velocity * m_lag;
            m_currentTime = 0;
        }
    }
}
