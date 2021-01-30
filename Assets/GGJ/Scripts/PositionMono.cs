using System;
using Photon.Pun;
using UnityEngine;

public class PositionMono : MonoBehaviour, IPunObservable
{
    public Vector3 m_velocity;
    public Vector3 m_start;
    public Vector3 m_end;
    public float m_currentTime = 0;
    public float m_lag;

    public void LateUpdate()
    {
        transform.position = Vector3.Lerp(m_start, m_end, m_currentTime / m_lag);
        m_currentTime += Time.deltaTime;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(m_velocity);
        }
        else
        {
            transform.position = (Vector3)stream.ReceiveNext();
            m_velocity = (Vector3) stream.ReceiveNext();

            m_start = m_end;
            m_lag = Mathf.Abs((float) (PhotonNetwork.Time - info.SentServerTime));
            m_end += m_velocity * m_lag;
            m_currentTime = 0;
        }
    }
}
