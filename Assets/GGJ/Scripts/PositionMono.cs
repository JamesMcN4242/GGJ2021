﻿using System;
using Photon.Pun;
using UnityEngine;

public class PositionMono : MonoBehaviour, IPunObservable
{
    public bool IsSeeker;
    
    [HideInInspector] public Vector3 m_velocity;
    private Vector3 m_start;
    private Vector3 m_end;
    private float m_currentTime = 0;
    private float m_lag;

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
            stream.SendNext(m_velocity);
        }
        else
        {
            Vector3 position = (Vector3)stream.ReceiveNext();
            m_velocity = (Vector3) stream.ReceiveNext();

            m_start = transform.position;
            m_lag = Mathf.Abs((float) (PhotonNetwork.Time - info.SentServerTime)) * 2f;
            m_end = position + m_velocity * m_lag;
            m_currentTime = 0;
        }
    }
}
