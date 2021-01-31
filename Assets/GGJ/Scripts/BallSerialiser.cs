using System;
using Photon.Pun;
using UnityEngine;

public class BallSerialiser : MonoBehaviour, IPunObservable
{
    private Rigidbody m_rigidbody;
    private Vector3 m_start;
    private Vector3 m_end;
    private float m_currentTime = 0;
    private float m_lag;
    
    private void Awake()
    {
        m_rigidbody = GetComponent<Rigidbody>();
    }

    public void LateUpdate()
    {
        if(Mathf.Abs(m_lag) > 0)
            transform.position = Vector3.Lerp(m_start, m_end, m_currentTime / m_lag);
        m_currentTime += Time.deltaTime;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(m_rigidbody);
        }
        else
        {
            Vector3 position = (Vector3)stream.ReceiveNext();
            m_rigidbody = (Rigidbody) stream.ReceiveNext();

            m_start = transform.position;
            m_lag = Mathf.Abs((float) (PhotonNetwork.Time - info.SentServerTime)) * 2f;
            m_end = position + m_rigidbody.velocity * m_lag;
            m_currentTime = 0;
        }
    }
}
